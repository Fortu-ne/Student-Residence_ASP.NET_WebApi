
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public class ManagerRep : IManager
    {
        private readonly AppDbContext _context;
    

        public ManagerRep(AppDbContext context)
        {
            _context = context;
           
        }

        public bool Create(Manager user)
        {
            _context.Managers.Add(user);
            return Save();
        }

        public Manager GetUserByEmail(string email)
        {
            return _context.Managers.FirstOrDefault(r => r.Email == email);
        }

        public Manager GetUser(string id)
        {
            return _context.Managers.FirstOrDefault(r => r.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UserExists(string id)
        {
            return _context.Managers.Any(r => r.Id == id);
        }

        public bool UserExistsByEmail(string searchItem)
        {
            return _context.Managers.Any(r => r.Email == searchItem);
        }

       public bool Update(Manager user)
        {
            _context.Update(user);
            return Save();
        }

       
    }


}