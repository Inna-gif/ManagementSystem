namespace ASP_proj.Models
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}