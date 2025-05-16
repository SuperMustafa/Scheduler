using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Repositories;

namespace Services.Schedulers
{
    public class ScheduleService(IScheduleRepository _scheduleRepository) : IScheduleService
    {


        public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            return schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                ScheduleName = s.ScheduleName,
                StartTime = s.StartTime.ToString(@"hh\:mm"),
                EndTime = s.EndTime.ToString(@"hh\:mm"),
                SelectedDays = s.SelectedDays,
                IsActive = s.IsActive
            });
        }
        public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null) return null;

            return new ScheduleDto
            {
                Id = schedule.Id,
                ScheduleName = schedule.ScheduleName,
                StartTime = schedule.StartTime.ToString(@"hh\:mm"),
                EndTime = schedule.EndTime.ToString(@"hh\:mm"),
                SelectedDays = schedule.SelectedDays,
                IsActive = schedule.IsActive
            };
        }
        public async Task<Schedule> CreateScheduleAsync(CreateScheduleDto dto)
        {
            var schedule = new Schedule
            {
                ScheduleName = dto.Name,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SelectedDays = dto.SelectedDays,
                IsActive = dto.IsActive,
                BuildingId= dto.BuildingId,
                ScheduleDevices =dto.ScheduleDevices.Select(sd => new ScheduleDevice
                {
                    DeviceId = sd.DeviceId,
                    Attributes = sd.Attributes.Select(a => new ScheduleDeviceAttribute
                    {
                        AttributeKey = a.AttributeKey,
                        AttributeValue = a.AttributeValue
                    }).ToList()
                }).ToList()
            };

           await _scheduleRepository.CreateAsync(schedule);


            return schedule;
        }

      

       

        public async Task<bool> UpdateScheduleAsync(int id, UpdateScheduleDto dto)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null) return false;

            schedule.ScheduleName = dto.Name;
            schedule.StartTime = TimeSpan.Parse(dto.StartTime);
            schedule.EndTime = TimeSpan.Parse(dto.EndTime);
            schedule.SelectedDays = dto.SelectedDays;
            schedule.IsActive = dto.IsActive;

            await _scheduleRepository.UpdateAsync(schedule);
            return true;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null) return false;

            await _scheduleRepository.DeleteAsync(schedule);
            return true;
        }

       
    }
}
