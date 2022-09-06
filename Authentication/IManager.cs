using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public interface IManager
    {
        bool Save();
        bool Create(Manager model);
        Manager GetUser(string id);
        Manager GetUserByEmail(string email);

        bool Update(Manager user);
        bool UserExists(string id);
        bool UserExistsByEmail(string searchItem);

      
    }


}
