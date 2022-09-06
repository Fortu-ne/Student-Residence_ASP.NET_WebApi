
namespace WepApiWithToken.ViewModels
{
    public class MaintenanceDto
    {
        public int MaintenanceId { get; set; }
        public string Description { get; set; }
        
        public string? MaintenanceDate { get; set; }

        public int StatusId { get; set; }
     
        public int MaintenanceTypeId { get; set; }
        public string StudId { get; set; }


    }
   

}
