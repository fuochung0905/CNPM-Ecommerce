namespace CNPM_ktxUtc2Store.Areas.Admin.dto
{
    public class variationdto
    {
        public int id {  get; set; }
            public string categoryname { get; set; }
        public List<variation> variations { get; set; }= new List<variation>();
        public string name { get; set; }
        public string value { get; set; }
    }
}
