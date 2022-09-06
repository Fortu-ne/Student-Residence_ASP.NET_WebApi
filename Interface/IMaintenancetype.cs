
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface IMaintenancetype
    {
        IEnumerable<MaintenanceType> GetAll();
        bool TypeExist(int id);
        bool Create(MaintenanceType type);
        bool Update(MaintenanceType type);
        bool Delete(MaintenanceType type);
        bool Save();

        MaintenanceType GetType(int id);
    }
}
