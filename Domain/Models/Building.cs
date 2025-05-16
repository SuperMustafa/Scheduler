using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Device> Devices { get; set; } = new List<Device>();

    }
}
