using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

namespace Services.Buildings
{
    public interface IBuildingService
    {
        Task<IEnumerable<BuildingDto>> GetAllAsync();
        Task<BuildingDto?> GetByIdAsync(int id);
        Task<BuildingDto> CreateAsync(BuildingDto dto);
        Task<BuildingDto?> UpdateAsync(int id, BuildingDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
