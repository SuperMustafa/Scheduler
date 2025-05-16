using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Persistance.Repositories
{
    public interface IBuildingRepository
    {
        Task<IEnumerable<Building>> GetAllAsync();
        Task<Building?> GetByIdAsync(int id);
        Task<Building> AddAsync(Building building);
        Task<Building> UpdateAsync(Building building);
        Task DeleteAsync(int id);
    }
}
