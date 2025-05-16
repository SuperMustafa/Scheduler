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
    public class BuildingRepository(SchedulerDbContext _dbConext) : IBuildingRepository
    {


        public async Task<IEnumerable<Building>> GetAllAsync()
        {
            return await _dbConext.Buildings.ToListAsync();
        }

        public async Task<Building?> GetByIdAsync(int id)
        {
            return await _dbConext.Buildings.FindAsync(id);
        }

        public async Task<Building> AddAsync(Building building)
        {
            _dbConext.Buildings.Add(building);
            await _dbConext.SaveChangesAsync();
            return building;
        }
        public async Task<Building> UpdateAsync(Building building)
        {
            _dbConext.Buildings.Update(building);
            await _dbConext.SaveChangesAsync();
            return building;
        }



        public async Task DeleteAsync(int id)
        {
            var building = await _dbConext.Buildings.FindAsync(id);
            if (building != null)
            {
                _dbConext.Buildings.Remove(building);
                await _dbConext.SaveChangesAsync();
            }
        }
    }
}
