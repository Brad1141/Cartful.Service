namespace Cartful.Service.Entities
{
    public class Account
    {
        public Guid userId {get; set;}
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string password {get; set;}
        public string phoneNumber { get; set; }
    }
}