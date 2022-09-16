
using Microsoft.EntityFrameworkCore;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Repository
{
    public class MaintenanceRep : IMaintenance
    {
        private readonly AppDbContext _applicationDbContext;

        public MaintenanceRep(AppDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool MaintenanceExist(int id)
        {
            return _applicationDbContext.Maintenances.Any(r => r.MaintenanceId == id);

        }

        public bool Create( Maintenance maintenance)
        {
          
            _applicationDbContext.Add(maintenance);
            return Save();
        }

        public bool Delete(Maintenance maintennce)
        {
            var model = GetMaintenance(maintennce.MaintenanceId);

            if (model != null)
            {
                _applicationDbContext.Maintenances.Remove(model);

            }

            return Save();
        }

        public Maintenance GetMaintenance(int id)
        {
            return _applicationDbContext.Maintenances.FirstOrDefault(r => r.MaintenanceId == id);
        }

        public IEnumerable<Maintenance> GetAll()
        {
            var query = _applicationDbContext.Maintenances.Include(r => r.MaintenanceType).Include(s=>s.Status).Include(r => r.Student).ThenInclude(r => r.Room).ToList();

            return query;
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Maintenance maintenance)
        {
            
            _applicationDbContext.Update(maintenance);
            return Save();
        }
    }

}
