using System;

namespace ASP_proj.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; } = null!;

        public int RouteId { get; set; }
        public Route Route { get; set; } = null!;

        public TimeSpan DepartureTime { get; set; }
        public string Status { get; set; } = "Active";
    }
}