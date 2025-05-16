using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class UpdateScheduleDto
    {
        public string Name { get; set; }
        public string StartTime { get; set; } // Format: "HH:mm"
        public string EndTime { get; set; }
        public string SelectedDays { get; set; }
        public bool IsActive { get; set; }
    }
}
