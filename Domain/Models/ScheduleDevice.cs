using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ScheduleDevice
    {
        public int Id { get; set; }
       public string AccessToken { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public ICollection<ScheduleDeviceAttribute> Attributes { get; set; }
    }
}
