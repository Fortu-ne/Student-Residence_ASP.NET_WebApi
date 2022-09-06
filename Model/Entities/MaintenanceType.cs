using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model.Entities
{
    public class MaintenanceType
    {
        [Key]
        public int MaintenanceTypeId { get; set; }
        public string Name { get; set; }
    }
}
