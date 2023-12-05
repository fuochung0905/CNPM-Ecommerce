namespace CNPM_ktxUtc2Store.Dto
{
    public class doneOrder
    {
        public int orderId { get; set; }
        public  List<shoppingCart> orderList { get; set; }= new List<shoppingCart>();
    }
}
