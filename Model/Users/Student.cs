using System.ComponentModel.DataAnnotations.Schema;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Model.Users
{
    public class Student : User
    {
        public Student()
        {
            Roles = "User";
        }
      
        public DateTime? BirthDate { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public string? ImageUrl { get; set; }
        public int? Age { get; set; }

        // relationship one-one

        
        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }

        [ForeignKey("RoomId")]
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
        [ForeignKey("CourseId")]
        public int? CourseId { get; set; }
        public Course? Course { get; set; }


        //relationship one-many
        public ICollection<Maintenance>? Maintenances { get; set; }
        public ICollection<CleaningService>? CleaningService { get; set; }
    }

    
}
