using System.ComponentModel.DataAnnotations;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Model.Entities
{
    public class Course
    {
        [Key]
        public int CourseTypeId { get; set; }
        public string CourseName { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
