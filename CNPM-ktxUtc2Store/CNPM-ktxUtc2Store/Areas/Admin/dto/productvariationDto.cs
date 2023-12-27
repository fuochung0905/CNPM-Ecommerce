namespace CNPM_ktxUtc2Store.Areas.Admin.dto
{
    public class productvariationDto
    {
        public int Id { get; set; } 
        public List<variation> Variations { get; set; } = new List<variation>();
        public List<productVariation>productVariations { get; set; }= new List<productVariation>(); 
    }
}
