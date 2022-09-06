using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{

    public interface IServiceManager
    {
        bool Save();
        IEnumerable<ServiceManagement> GetAll();

        bool Create(ServiceManagement model);
        bool Delete(ServiceManagement model);
        bool Update(ServiceManagement user);
        ServiceManagement GetUser(string id);
        ServiceManagement GetUserByEmail(string email);
        bool UserExists(string id);
        bool UserExistByEmail(string email);
    }


}
