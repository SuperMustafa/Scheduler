using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SelectedDays { get; set; }
        public bool IsActive { get; set; }
    }
}
