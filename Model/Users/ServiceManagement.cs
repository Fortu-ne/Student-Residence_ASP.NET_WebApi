using WepApiWithToken.Model.Entities;

namespace WepApiWithToken.Model.Users
{
    public class ServiceManagement : User
    {
        public ServiceManagement()
        {
            Roles = "Service Management";
        }
     
        public DateTime? DateHired { get; set; }


        //public ICollection<Maintenance> Maintenances { get; set; }
        //public ICollection<CleaningService> CleaningServices { get; set; }
    }
}
