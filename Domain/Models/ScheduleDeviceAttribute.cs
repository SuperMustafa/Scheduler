using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ScheduleDeviceAttribute
    {
        public int Id { get; set; }

        public int ScheduleDeviceId { get; set; }
        public ScheduleDevice ScheduleDevice { get; set; }

        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }
    }
}
