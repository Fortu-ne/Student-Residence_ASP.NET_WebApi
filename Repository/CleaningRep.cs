
using Microsoft.EntityFrameworkCore;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Repository
{
    public class CleaningRep : ICleaning
    {
        private readonly AppDbContext _context;

        public CleaningRep(AppDbContext context)
        {
            _context = context;
        }

        public bool CLeaningExist(int id)
        {
            return _context.Cleanings.Any(r => r.CleaningId == id);
        }

        public bool Create( CleaningService model)
        {
            
            _context.Cleanings.Add(model);

            return Save();
        }

        public bool Delete(CleaningService service)
        {
            var model = GetCleaningService(service.CleaningId);

            if (model != null)
            {
                _context.Cleanings.Remove(model);
            }

            return Save();
        }

        public IEnumerable<CleaningService> GetAll()
        {
            return _context.Cleanings.Include(r=>r.CleaningType).Include(r => r.Student).ThenInclude(r=>r.Room).ToList();
        }

        public IEnumerable<CleaningType> GetAllTypes()
        {
            return _context.CleaningTypes.ToList();
        }

        public CleaningService GetCleaningService(int id)
        {
            return _context.Cleanings.FirstOrDefault(r => r.CleaningId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(CleaningService model)
        {
            _context.Update(model);

            return Save();
        }
    }

}
