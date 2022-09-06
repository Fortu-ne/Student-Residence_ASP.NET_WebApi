

using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface IStatus
    {
        IEnumerable<StatusType> GetAll();
        bool StatusExist(int id);
        bool Create(StatusType status);
        bool Update(StatusType status);
        bool Delete(StatusType status);
        bool Save();

        StatusType GetStatus(int id);
    }
}
