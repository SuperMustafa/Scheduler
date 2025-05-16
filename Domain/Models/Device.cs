using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public string UnitType { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public string AccessToken { get; set; }

        public ICollection<ScheduleDevice> ScheduleDevices { get; set; }
    }
}
