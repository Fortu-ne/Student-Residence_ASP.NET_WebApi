using System.ComponentModel.DataAnnotations;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Model.Entities
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }
        public string GenderType { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Room> Rooms { get; set; }

    }
}
