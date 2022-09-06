
using WepApiWithToken.Model;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public class ServiceRep : IServiceManager
    {
        private readonly AppDbContext _context;


        public ServiceRep(AppDbContext context)
        {
            _context = context;

        }

        public bool Create(ServiceManagement user)
        {
            _context.ServiceManagements.Add(user);
            return Save();
        }

        public bool Delete(ServiceManagement model)
        {
            var user = GetUser(model.Id);

            if(user != null)
            {
                _context.Remove(model);
                return Save();
            }

            return Save();
        }

        public IEnumerable<ServiceManagement> GetAll()
        {
            return _context.ServiceManagements.ToList();
        }

        public ServiceManagement GetUser(string id)
        {
            return _context.ServiceManagements.FirstOrDefault(r => r.Id == id);
        }

        public ServiceManagement GetUserByEmail(string email)
        {
            return _context.ServiceManagements.FirstOrDefault(r => r.Email == email);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ServiceManagement user)
        {
            _context.Update(user);
            return Save();
        }

    

        public bool UserExistByEmail(string email)
        {
            return _context.ServiceManagements.Any(r => r.Email == email);
        }

        public bool UserExists(string id)
        {
            return _context.ServiceManagements.Any(r => r.Id == id);
        }
    }


}