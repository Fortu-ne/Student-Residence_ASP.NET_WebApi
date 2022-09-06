

namespace WepApiWithToken.ViewModels
{
    public class RoomDto { 
        public int RoomId { get; set; }
        public string RoomNum { get; set; }
        public string FloorNumber { get; set; }
             //One-to-One
        public int GenderId { get; set; }

    }
}

