namespace CNPM_ktxUtc2Store.Areas.Admin.dto
{
    public class productDto
    {
        public int Id { get; set; }
        public List<product>products { get; set; }  = new List<product>();
        public List<productVariation> productVariation { get; set; }= new List<productVariation> ();
    }
}
