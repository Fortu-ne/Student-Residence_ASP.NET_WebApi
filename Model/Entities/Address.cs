namespace WepApiWithToken.Model.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
    }
}
