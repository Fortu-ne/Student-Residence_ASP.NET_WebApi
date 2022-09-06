using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.ViewModels
{
    public class StudentDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        //public IFormFile? ImageUrl { get; set; }
        public int Age { get; set; }
        public int GenderId { get; set; }
        public string PhoneNumber { get; set; }
        public int RoomId { get; set; }
        public int CourseId { get; set; }
        public Address? Address { get; set; }

    }
}

