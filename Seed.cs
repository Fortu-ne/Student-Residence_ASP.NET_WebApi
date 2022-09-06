using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken
{
    public class Seed
    {
        private readonly AppDbContext applicationDbContext;

        public Seed(AppDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void SeedDataContext()
        {
            if (!applicationDbContext.CleaningTypes.Any())
            {
                var modelList = new List<CleaningType>()
                {
                    new CleaningType()
                    {

                        Name = "Room",
                    },
                    new CleaningType()
                    {

                        Name = "Deep Cleaning",
                    },
                    new CleaningType()
                    {

                        Name = "Laundry"
                    },
                    new CleaningType()
                    {

                        Name = "Ironing",
                    },
                };
                applicationDbContext.CleaningTypes.AddRange(modelList);
                applicationDbContext.SaveChanges();
            }

            if (!applicationDbContext.StatusTypes.Any())
            {
                var statusList = new List<StatusType>()
                {
                    new StatusType()
                    {
                        Name = "Pending",
                    },
                     new StatusType()
                    {
                     Name="Assigned",
                    },
                      new StatusType()
                    {
                        Name = "Decline",
                    },
                };
                applicationDbContext.StatusTypes.AddRange(statusList);
                applicationDbContext.SaveChanges();
            }

            if (!applicationDbContext.Genders.Any())
            {
                var gendersList = new List<Gender>()
              {
                  new Gender
                  {
                      GenderType = "Male",
                      Rooms = new List<Room>()
                      {
                       new Room { RoomNum = "A201", FloorNumber = "Ground Floor"},
                       new Room { RoomNum = "A202", FloorNumber = "Ground Floor"},
                       new Room { RoomNum = "A203", FloorNumber = "Ground Floor"},
                       new Room { RoomNum = "A204", FloorNumber = "Ground Floor"},
                       new Room { RoomNum = "C201", FloorNumber = "2nd Floor"},
                       new Room { RoomNum = "C202", FloorNumber = "2nd Floor"},
                       new Room { RoomNum = "C203", FloorNumber = "2nd Floor"},
                       new Room { RoomNum = "C204", FloorNumber = "2nd Floor"},
                       new Room { RoomNum = "D204", FloorNumber = "3rd Floor"},
                      },
                  },
                   new Gender
                  {
                      GenderType = "Female",
                      Rooms = new List<Room>()
                      {
                           new Room { RoomNum = "D203", FloorNumber = "3rd Floor"},
                           new Room { RoomNum = "D202", FloorNumber = "3rd Floor"},
                           new Room { RoomNum = "D201", FloorNumber = "3rd Floor"},
                           new Room { RoomNum = "B101", FloorNumber = "1st Floor"},
                           new Room { RoomNum = "B102", FloorNumber = "1st Floor"},
                           new Room { RoomNum = "B103", FloorNumber = "1st Floor"},
                           new Room { RoomNum = "B104",FloorNumber = "1st Floor" },
                      },

                  },
              };
                applicationDbContext.Genders.AddRange(gendersList);
                applicationDbContext.SaveChanges();
            };

            if (!applicationDbContext.Courses.Any())
            {
                var courseTypes = new List<Course>()
                {
                    new Course()
                    {
                        CourseName = "Information Technology",
                    },
                    new Course()
                    {
                        CourseName = "Computer Sciences",
                    },
                     new Course()
                    {

                        CourseName = "Data Science",
                    },
                    new Course()
                    {

                        CourseName = "Art",
                    },
                     new Course()
                    {

                        CourseName = "Teaching",
                    },
                    new Course()
                    {
                        CourseName = "Phyiscal Science",
                    },
                };
                applicationDbContext.Courses.AddRange(courseTypes);
                applicationDbContext.SaveChanges();
            };

            if (!applicationDbContext.MaintenanceTypes.Any())
            {
                var maintenanceTypes = new List<MaintenanceType>()
                {
                   new MaintenanceType()
                   {
                      Name = "Elctrical",
                   },
                   new MaintenanceType()
                   {
                      Name = "Plumbing",
                   },
                   new MaintenanceType()
                   {
                      Name = "Damage",
                   },
                   };
                applicationDbContext.MaintenanceTypes.AddRange(maintenanceTypes);
                applicationDbContext.SaveChanges();
            };
        }
    }
}
