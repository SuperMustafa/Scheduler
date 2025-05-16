using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Models;

namespace Services.Schedulers
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
        Task<ScheduleDto?> GetScheduleByIdAsync(int id);

        Task<Schedule> CreateScheduleAsync(CreateScheduleDto dto);
        Task<bool> UpdateScheduleAsync(int id, UpdateScheduleDto dto);
        Task<bool> DeleteScheduleAsync(int id);
       

    }
}
