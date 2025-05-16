
using Domain.Dtos;
using Domain.Models;
using Persistance.Repositories;
using Services.Buildings;

namespace Services.Buildings
{
    public class BuildingService(IBuildingRepository _buildingRepository) : IBuildingService
    {
       

        public async Task<IEnumerable<BuildingDto>> GetAllAsync()
        {
            var buildings = await _buildingRepository.GetAllAsync();
            return buildings.Select(b => new BuildingDto
            {
                Id = b.Id,
                Name = b.Name
            });
        }

        public async Task<BuildingDto?> GetByIdAsync(int id)
        {
            var building = await _buildingRepository.GetByIdAsync(id);
            if (building == null) return null;

            return new BuildingDto { Id = building.Id, Name = building.Name };
        }
        public async Task<BuildingDto> CreateAsync(BuildingDto dto)
        {
            var building = new Building { Name = dto.Name };
            var result = await _buildingRepository.AddAsync(building);
            return new BuildingDto { Id = result.Id, Name = result.Name };
        }

        public async Task<BuildingDto?> UpdateAsync(int id, BuildingDto dto)
        {
            var building = await _buildingRepository.GetByIdAsync(id);
            if (building == null) return null;

            building.Name = dto.Name;
            var updated = await _buildingRepository.UpdateAsync(building);
            return new BuildingDto { Id = updated.Id, Name = updated.Name };
        }

     

        public async Task<bool> DeleteAsync(int id)
        {
            var b = await _buildingRepository.GetByIdAsync(id);
            if (b == null) return false;

            await _buildingRepository.DeleteAsync(id);
            return true;
        }
    }
}
