using System.ComponentModel.DataAnnotations;

namespace WepApiWithToken.Model.Entities
{
    public class StatusType
    {
        [Key]
        public int StatusId { get; set; }
        public string Name { get; set; }
    }
}
