using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class CreateScheduleDto
    {
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; } // Format: "HH:mm"
        public TimeSpan EndTime { get; set; }
        public string SelectedDays { get; set; }
        public bool IsActive { get; set; }
        public int BuildingId { get; set; }
        public List<ScheduleDeviceCreateDto> ScheduleDevices { get; set; }
    }
}
