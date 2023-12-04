namespace CNPM_ktxUtc2Store.Models
{
    public class Adress
    {
        public int Id { get; set; }
        public string homeAdress { get; set; }
        public string villageAdress { get; set;}
        public string districAdress { get; set; }
        public virtual List<UserAdress> UserAdresses { get; set; } = new List<UserAdress>();


    }
}
