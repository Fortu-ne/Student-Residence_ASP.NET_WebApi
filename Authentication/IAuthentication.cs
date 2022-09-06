using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Authentication
{
    public interface IAuthentication<T>
    {
        void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt);
        bool VerifyPasswordHash(string password, byte[] PasswordHash,  byte[] PasswordSalt);

        string CreateToken(T user);
       
        string CreateRandomToken();
    }
}
