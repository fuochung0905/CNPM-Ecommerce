namespace CNPM_ktxUtc2Store.Areas.Admin.dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public List<applicationUser> Users { get; set; }=new List<applicationUser>();
    }
}
