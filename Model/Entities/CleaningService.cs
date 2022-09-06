using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WepApiWithToken.Model.Users;

namespace WepApiWithToken.Model.Entities
{
    public class CleaningService
    {
        [Key]
        public int CleaningId { get; set; }

        public string? Description { get; set; }
        public string? CleaningDate { get; set; }

        [ForeignKey("CleaningTypeId")]
        public int CleaningTypeId { get; set; }
        public CleaningType CleaningType { get; set; }

        [ForeignKey("StudentId")]
        public string? StudId { get; set; }
        public Student? Student { get; set; }

    }
}
