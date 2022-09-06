
using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Interface
{
    public interface IAddress
    {
        IEnumerable<Address> GetAll();
        bool Create(Address address);
        bool Update(Address address);
        bool Delete(Address adress);
        bool AddressExist(int id);
        bool Save();

        Address GetAddress(int id);
    }
}
