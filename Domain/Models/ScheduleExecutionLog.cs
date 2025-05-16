using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ScheduleExecutionLog
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public string DeviceName { get; set; }
        public string AttributesSent { get; set; } // JSON string of attributes
        public DateTime SentAt { get; set; }

        public Schedule Schedule { get; set; } // Navigation property
    }
}
