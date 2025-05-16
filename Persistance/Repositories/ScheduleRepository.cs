using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly SchedulerDbContext _DbContext;
        public ScheduleRepository(SchedulerDbContext DbContext)
        {
            _DbContext= DbContext;
        }

       
        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _DbContext.Schedules.ToListAsync();
        }

        public async Task<Schedule?> GetByIdAsync(int id)
        {
            return await _DbContext.Schedules.FindAsync(id);
        }

        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            _DbContext.Schedules.Add(schedule);
            await _DbContext.SaveChangesAsync();
            return schedule;
        }


        public async Task UpdateAsync(Schedule schedule)
        {
            _DbContext.Schedules.Update(schedule);
            await _DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Schedule schedule)
        {
            _DbContext.Schedules.Remove(schedule);
            await _DbContext.SaveChangesAsync();
        }
    }
}
