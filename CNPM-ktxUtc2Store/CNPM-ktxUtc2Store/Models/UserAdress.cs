namespace CNPM_ktxUtc2Store.Models
{
    public class UserAdress
    {
        public int AdressId { get; set; }
        public Adress adress { get; set; }
        public string applicationUserId { get; set; }
        public virtual applicationUser applicationUser { get; set; }
        public bool isDefine { get; set; }


      
    }
}
