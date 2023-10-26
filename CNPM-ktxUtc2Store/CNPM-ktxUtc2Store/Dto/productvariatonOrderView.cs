using Microsoft.AspNetCore.Mvc.Rendering;

namespace CNPM_ktxUtc2Store.Dto
{
    public class productvariatonOrderView
    {
        public int productId { get; set; }
        public product product { get; set; }
       
        public int quantity { get; set; }
        public string size { get; set; }
        public string color { get; set; }

        

    }
}
