
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface ICleaning
    {
        IEnumerable<CleaningService> GetAll();
        IEnumerable<CleaningType> GetAllTypes();
        bool Create(/*int typeId, string studId, */CleaningService model);
        bool Update(/*int typeId, string studId,*/ CleaningService model);
        bool Delete(CleaningService adress);
        bool CLeaningExist(int id);
        bool Save();

        CleaningService GetCleaningService(int id);
    }
}
