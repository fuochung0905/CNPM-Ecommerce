namespace CNPM_ktxUtc2Store.Areas.Admin.dto
{
    public class dask
    {
        public int sonhanvien {  get; set; }
        public int songuoidung { get; set; }
        public double tongdoanhso { get; set; }
        public double doanhsothang { get; set; }
        public double tongnhaphang { get; set; }
        public List<orderDetail> orderDetail { get; set; }=new List<orderDetail>();
        public List<order> order { get; set; } = new List<order>();
    }
}
