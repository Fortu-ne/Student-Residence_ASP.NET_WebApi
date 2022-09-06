using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Model.Entities
{
    public class Maintenance
    {

        [Key]
        public int MaintenanceId { get; set; }
        public string Description { get; set; }

        public string? MaintenanceDate { get; set; }

        // One to One Relationship
        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }
        public StatusType? Status { get; set; }

        [ForeignKey("MaintenanceType")]
        public int? MaintenanceTypeId { get; set; }
        public MaintenanceType? MaintenanceType { get; set; }

        [ForeignKey("StudentId")]
        public string? StudId { get; set; }
        public Student? Student { get; set; }

      
    }
}
