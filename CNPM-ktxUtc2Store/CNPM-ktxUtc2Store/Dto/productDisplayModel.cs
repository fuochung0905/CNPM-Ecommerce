namespace CNPM_ktxUtc2Store.Dto
{
    public class productDisplayModel
    {
        public IEnumerable<product>products { get; set; }
        public IEnumerable<category> categories { get; set; }
        public string sterm { get; set; } = "";
        public int categoryId { get; set; } = 0;
    }

}
