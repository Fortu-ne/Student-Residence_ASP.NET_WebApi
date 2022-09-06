using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model.Entities
{
    public class CleaningType
    {
        [Key]
        public int CleaningTypeId { get; set; }
        public string Name { get; set; }
    }
}
