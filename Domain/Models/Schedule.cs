namespace Domain.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
       
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string SelectedDays { get; set; } // e.g. "Mon,Tue"
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public ICollection<ScheduleDevice> ScheduleDevices { get; set; }

    }
}
