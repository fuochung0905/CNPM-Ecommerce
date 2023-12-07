namespace CNPM_ktxUtc2Store.Areas.Admin.dto
{
    public class duyetDon
    {
        public int Id { get; set; }
        public orderDetail orderDetail { get; set; }
        public string userId { get; set; }
        public List<order> orderList { get; set; } = new List<order>();
    }
}
