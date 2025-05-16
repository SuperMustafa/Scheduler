using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ScheduleDeviceCreateDto
    {
        public int DeviceId { get; set; }
        public List<ScheduleDeviceAttributeCreateDto> Attributes { get; set; }
    }
}
