using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    public class productvoption
    {
        [Key, Column(Order = 0)]
        public int productId { get; set; }

        [Key, Column(Order = 1)]
        public int variationoptionId { get; set; }
        public virtual product product { get; set; }
        public virtual variation_option variationoption { get; set; }   
    }
}
