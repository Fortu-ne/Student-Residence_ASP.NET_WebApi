
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface IRooms
    {
        IEnumerable<Room> GetAll();
        bool Create(int GenderId, Room room);
        bool Update(int GenderId, Room room);
        bool Delete(Room room);
        bool RoomExist(int id);
        bool Save();
        IEnumerable<Room> GeyByGender(int cid);
        Room GetRoom(int id);
    }
}
