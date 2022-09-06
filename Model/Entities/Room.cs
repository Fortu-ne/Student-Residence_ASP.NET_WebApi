using System.ComponentModel.DataAnnotations;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Model.Entities
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomNum { get; set; }
        public string FloorNumber { get; set; }
        public bool SharingRoom { get; set; }

        //One-to-One
        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        // relationship one-many

        public ICollection<Student> Students { get; set; }

    }
}
