
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Repository
{
    public class MaintenanceTypeRep : IMaintenancetype
    {
        private readonly AppDbContext _db;

        public MaintenanceTypeRep(AppDbContext db)
        {
            _db = db;
        }

        public bool Create(MaintenanceType type)
        {
            _db.MaintenanceTypes.Add(type);
            return Save();
        }

        public bool Delete(MaintenanceType type)
        {
            var model = GetType(type.MaintenanceTypeId);

            if (model != null)
            {
                _db.MaintenanceTypes.Remove(model);

            }
            return Save();
        }

        public IEnumerable<MaintenanceType> GetAll()
        {
            var query = _db.MaintenanceTypes.ToList();

            return query;
        }

        public MaintenanceType GetType(int id)
        {
            return _db.MaintenanceTypes.FirstOrDefault(r => r.MaintenanceTypeId == id);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TypeExist(int id)
        {
            return _db.MaintenanceTypes.Any(r => r.MaintenanceTypeId == id);
        }

        public bool Update(MaintenanceType type)
        {
            _db.MaintenanceTypes.Update(type);
            return Save();
        }
    }
}
