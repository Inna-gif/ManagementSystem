namespace ASP_proj.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public int Number { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}