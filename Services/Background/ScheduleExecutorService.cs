using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using System;
using Persistance.Data;
using Domain.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Services.Background
{
    public class ScheduleExecutorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduleExecutorService> _logger;
        private readonly HttpClient _httpClient;
        public ScheduleExecutorService(IServiceProvider serviceProvider, ILogger<ScheduleExecutorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduler started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();

                    var now = DateTime.Now;
                    var currentTime = now.TimeOfDay;
                    var today = now.DayOfWeek.ToString();

                    var schedules = await db.Schedules
                        .Where(s => s.IsActive && s.SelectedDays.Contains(today)
                                    && s.StartTime.Hours == currentTime.Hours
                                    && s.StartTime.Minutes == currentTime.Minutes)
                        .Include(s => s.ScheduleDevices)
                            .ThenInclude(sd => sd.Device)
                        .Include(s => s.ScheduleDevices)
                            .ThenInclude(sd => sd.Attributes)
                        .ToListAsync();

                    foreach (var schedule in schedules)
                    {
                        foreach (var scheduleDevice in schedule.ScheduleDevices)
                        {
                            var device = scheduleDevice.Device;
                            var attributes = scheduleDevice.Attributes.ToDictionary(a => a.AttributeKey, a => (object)a.AttributeValue);

                            // Send to ThingsBoard REST API
                            var token = device.AccessToken; // Make sure AccessToken is a property
                            var url = $"http://localhost:8080/api/v1/{token}/attributes"; // Adjust host if needed

                            var response = await _httpClient.PostAsJsonAsync(url, attributes, stoppingToken);

                            if (response.IsSuccessStatusCode)
                            {
                                _logger.LogInformation($"Updated device {device.UnitName} for schedule {schedule.ScheduleName}");
                            }

                            else
                            {
                                _logger.LogWarning($"Failed to update device {device.UnitName}: {response.StatusCode}");
                            }

                            var log = new ScheduleExecutionLog
                            {
                                ScheduleId = schedule.Id,
                                DeviceName = device.UnitName,
                                AttributesSent = JsonConvert.SerializeObject(attributes),
                                SentAt = DateTime.Now
                            };

                            db.ScheduleExecutionLogs.Add(log);
                            await db.SaveChangesAsync();
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Wait for 1 min
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing schedule");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Wait before retry
                }
            }
        }
    }
}
