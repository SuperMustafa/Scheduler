using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Data;

public class ScheduleExecutorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ScheduleExecutorService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ScheduleExecutorService(
        IServiceScopeFactory scopeFactory,
        ILogger<ScheduleExecutorService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ScheduleExecutorService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();

            var dueSchedules = await dbContext.Schedules
                .Where(s => s.StartTime <= DateTime.Now.TimeOfDay && !s.IsActive)
                .ToListAsync(stoppingToken);

            foreach (var schedule in dueSchedules)
            {
                try
                {
                    var success = await SendAttributeToThingsBoardAsync(schedule);

                    if (success)
                    {
                        schedule.IsActive = true;
                        _logger.LogInformation($"Executed schedule ID {schedule.Id} for device {schedule.ScheduleDevices.Select(sd=>sd.DeviceId)}.");
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to send attribute for schedule ID {schedule.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error executing schedule ID {schedule.Id}");
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Run every 30 seconds
        }
    }

    private async Task<bool> SendAttributeToThingsBoardAsync(Schedule schedule)
    {
        var thingsBoardBaseUrl = _configuration["ThingsBoard:BaseUrl"];
        var jwtToken = _configuration["ThingsBoard:JwtToken"];

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        bool allSuccess = true;

        foreach (var device in schedule.ScheduleDevices)
        {
            if (string.IsNullOrWhiteSpace(device.AccessToken))
            {
                _logger.LogWarning($"Device {device.DeviceId} has no access token, skipping.");
                continue;
            }

            var attributesDict = device.Attributes.ToDictionary(attr => attr.AttributeKey, attr => attr.AttributeValue);

            if (attributesDict.Count == 0)
                continue;  // Skip devices with no attributes

            var url = $"{thingsBoardBaseUrl}/api/v1/{device.AccessToken}/attributes";

            var content = new StringContent(JsonSerializer.Serialize(attributesDict), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                allSuccess = false;
                _logger.LogError($"Failed to send attributes to device {device.DeviceId} ({device.AccessToken}): {response.StatusCode}");
            }
            else
            {
                _logger.LogInformation($"Successfully sent attributes to device {device.DeviceId} ({device.AccessToken})");
            }
        }

        return allSuccess;
    }


}
