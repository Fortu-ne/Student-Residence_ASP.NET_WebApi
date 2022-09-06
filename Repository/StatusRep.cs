
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Repository
{
    public class StatusRep : IStatus
    {
        private readonly AppDbContext _dbData;

        public StatusRep(AppDbContext dbData)
        {
            _dbData = dbData;
        }

        public bool Create(StatusType status)
        {
            _dbData.StatusTypes.Add(status);
            return Save();
        }

        public bool Delete(StatusType status)
        {
            var model = GetStatus(status.StatusId);

            if (model != null)
            {
                _dbData.Remove(model);

            }

            return Save();
        }

        public IEnumerable<StatusType> GetAll()
        {
            var query = _dbData.StatusTypes.ToList();

            return query;
        }

        public StatusType GetStatus(int id)
        {
            return _dbData.StatusTypes.FirstOrDefault(r => r.StatusId == id);
        }

        public bool Save()
        {
            var saved = _dbData.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool StatusExist(int id)
        {
            return _dbData.StatusTypes.Any(r => r.StatusId == id);
        }

        public bool Update(StatusType status)
        {
            _dbData.Update(status);
            return Save();
        }
    }
}
