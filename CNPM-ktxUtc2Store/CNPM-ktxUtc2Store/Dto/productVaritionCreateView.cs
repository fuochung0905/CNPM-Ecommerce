using Microsoft.AspNetCore.Mvc.Rendering;

namespace CNPM_ktxUtc2Store.Dto
{
    public class productVaritionCreateView
    {
        public List<SelectListItem> Items { get; set; }
        public string[] selectVariation { get; set; }
    }
}
