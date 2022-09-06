namespace WepApiWithToken.Model.Users
{
    public class Manager : User
    {
        public Manager()
        {
            Roles = "Admin";
        }
        
        public DateTime? DateAppointed { get; set; }
    }
}
