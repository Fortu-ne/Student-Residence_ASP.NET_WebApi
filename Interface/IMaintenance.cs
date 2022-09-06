

using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface IMaintenance
    {
        IEnumerable<Maintenance> GetAll();
        bool Create(/*int typeId, string studIdint statusId,*/ Maintenance address);
        bool Update(/*int typeId, string studId, int statusId,*/ Maintenance adress);
        bool Delete(Maintenance adress);
        bool MaintenanceExist(int id);
        bool Save();

        Maintenance GetMaintenance(int id);
    }
}
