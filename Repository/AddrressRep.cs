
using Microsoft.EntityFrameworkCore;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Repository
{
    public class AddressRep : IAddress
    {
        private readonly AppDbContext _db;

        public AddressRep(AppDbContext db)
        {
            _db = db;
        }

        public bool AddressExist(int id)
        {
            return _db.Addresses.Any(r => r.AddressId == id);
        }

        public bool Create(Address address)
        {

            _db.Addresses.Add(address);
            return Save();
        }

        public bool Delete(Address address)
        {
            var model = GetAddress(address.AddressId);

            if (model != null)
            {
                _db.Addresses.Remove(model);
                return Save();
            }
            return Save();

        }


        public Address GetAddress(int id)
        {
            return _db.Addresses.FirstOrDefault(r => r.AddressId == id);
        }

        public IEnumerable<Address> GetAll()
        {
            return _db.Addresses/*.Whe/*re(c => c.Student.Name == name || string.IsNullOrWhiteSpace(name)).Include(t => t.Student).Include(r=>r.Student)*/.ToList();
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Address address)
        {
            //address.StudentId = studId;
            _db.Update(address);
            return Save();
        }
    }
}
