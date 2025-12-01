using Microsoft.EntityFrameworkCore;

namespace ASP_proj.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>().HasKey(d => d.DriverId);
            modelBuilder.Entity<Route>().HasKey(r => r.RouteId);
            modelBuilder.Entity<Schedule>().HasKey(s => s.ScheduleId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Driver)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Route)
                .WithMany(r => r.Schedules)
                .HasForeignKey(s => s.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Driver>().HasData(
                new Driver { DriverId = 1, Name = "Ivan Petrenko", Email = "ivan@example.com", PasswordHash = "pass1" },
                new Driver { DriverId = 2, Name = "Olena Shevchenko", Email = "olena@example.com", PasswordHash = "pass2" }
            );

            modelBuilder.Entity<Route>().HasData(
                new Route { RouteId = 1, Number = 5, StartPoint = "Center", EndPoint = "Railway station" },
                new Route { RouteId = 2, Number = 12, StartPoint = "Market", EndPoint = "University" }
            );

            modelBuilder.Entity<Schedule>().HasData(
                new Schedule
                {
                    ScheduleId = 1,
                    DriverId = 1,
                    RouteId = 1,
                    DepartureTime = new TimeSpan(8, 0, 0),
                    Status = "OnTime"
                },
                new Schedule
                {
                    ScheduleId = 2,
                    DriverId = 2,
                    RouteId = 2,
                    DepartureTime = new TimeSpan(9, 30, 0),
                    Status = "Delayed"
                }
            );
        }
    }
}