namespace CNPM_ktxUtc2Store.Dto
{
    public class doneOrder
    {
        public int orderId { get; set; }
        public bool check {  get; set; }    
        public  List<order> orderList { get; set; }= new List<order>();
    }
}
