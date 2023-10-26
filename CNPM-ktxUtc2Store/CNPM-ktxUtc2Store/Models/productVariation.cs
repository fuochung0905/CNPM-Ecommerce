namespace CNPM_ktxUtc2Store.Models
{
    public class productVariation
    {
        public int productId { get; set; }
        public virtual product product { get; set; }
        public int variationId { get; set;}
        public virtual variation variation { get; set;}
      
    }
}
