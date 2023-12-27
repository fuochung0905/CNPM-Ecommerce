namespace CNPM_ktxUtc2Store.Dto
{
    public class myOrderDto
    {
        public int Id { get; set; }
        public List<orderDetail> orderDetails { get; set; } = new List<orderDetail>();
    }
}
