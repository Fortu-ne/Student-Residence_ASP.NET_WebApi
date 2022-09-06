
using Microsoft.EntityFrameworkCore;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace JulyProject.Repositroy
{
    public class RoomRep : IRooms
    {
        private readonly AppDbContext _dbContext;

        public RoomRep(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(int GenderId,Room room)
        {
            _dbContext.Rooms.Add(room);
            room.GenderId = GenderId;
            return Save();
        }

        public bool Delete(Room room)
        {
            var model = GetRoom(room.RoomId);

            if(model!= null)
            {
                _dbContext.Remove(model);

            }

            return Save();
        }

        public IEnumerable<Room> GetAll()
        {
            var query = _dbContext.Rooms.Include(r=>r.Gender).ToList();
            return query;
        }

        public Room GetRoom(int id)
        {
            return _dbContext.Rooms.FirstOrDefault(r => r.RoomId == id);
        }

        public bool RoomExist(int id)
        {
           return _dbContext.Rooms.Any(r => r.RoomId == id);
           
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;    
        }

        public bool Update(int GenderId, Room room)
        {
            _dbContext.Update(room);
            room.GenderId = GenderId;
            return Save();
        }

        public IEnumerable<Room> GeyByGender(int cid)
        {
            return _dbContext.Rooms.Where(r => r.GenderId == cid).ToList();
        }
    }
}
