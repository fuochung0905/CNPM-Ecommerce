namespace CNPM_ktxUtc2Store.Dto
{
    public class changeRole
    {
        public string Id { get; set; }
        public List<applicationUser> applicationUsers { get; set; }= new List<applicationUser>();
       
    }
}
