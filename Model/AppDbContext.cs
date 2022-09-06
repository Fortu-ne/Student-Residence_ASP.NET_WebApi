using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

     
     
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<CleaningService> Cleanings { get; set; }
        public DbSet<CleaningType> CleaningTypes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<MaintenanceType> MaintenanceTypes { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<ServiceManagement> ServiceManagements { get; set; }
        public DbSet<Manager> Managers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server =.\\SQLExpress03; Database = Authentication; Trusted_Connection = true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           


            modelBuilder.Entity<Student>()
              .HasMany(c => c.Maintenances)
              .WithOne(c => c.Student)
              .HasForeignKey(c => c.StudId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasMany(c => c.CleaningService)
                .WithOne(c => c.Student)
                .HasForeignKey(c => c.StudId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
           .HasMany(c => c.Maintenances)
            .WithOne(e => e.Student);

            modelBuilder.Entity<Course>()
               .HasMany(c => c.Students)
               .WithOne(c => c.Course)
               .HasForeignKey(c => c.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gender>()
                .HasMany(c => c.Students)
                .WithOne(c => c.Gender)
                .HasForeignKey(c => c.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
             .HasMany(c => c.Students)
             .WithOne(e => e.Room).HasForeignKey(c => c.RoomId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gender>().HasMany(c => c.Rooms).WithOne(c => c.Gender).HasForeignKey(r => r.GenderId);



        }

    }
}
