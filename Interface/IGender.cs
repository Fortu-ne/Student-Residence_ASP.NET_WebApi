

using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface IGender
    {
        IEnumerable<Gender> GetAll();
        bool GenderExist(int id);
        bool Create(Gender gender);
        bool Update(Gender gender);
        bool Delete(Gender gender);
        bool Save();

        Gender GetGender(int id);
    }
}
