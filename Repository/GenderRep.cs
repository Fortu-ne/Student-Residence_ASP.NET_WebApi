
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Repository
{
    public class GenderRep : IGender
    {
        private readonly AppDbContext _db;

        public GenderRep(AppDbContext db)
        {
            _db = db;
        }

        public bool Create(Gender gender)
        {
            _db.Genders.Add(gender);
            return Save();
        }

        public bool Delete(Gender gender)
        {
            var model = GetGender(gender.GenderId);

            if (model != null)
            {
                _db.Remove(model);

            }

            return Save();

        }

        public bool GenderExist(int id)
        {
            return _db.Genders.Any(r => r.GenderId == id);
        }

        public IEnumerable<Gender> GetAll()
        {
            var query = _db.Genders.ToList();

            return query;
        }

        public Gender GetGender(int id)
        {
            return _db.Genders.FirstOrDefault(r => r.GenderId == id);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Gender gender)
        {
            _db.Update(gender);
            return Save();
        }
    }
}
