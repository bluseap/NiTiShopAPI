using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.ViewModels.CShop
{
    public class ClientHomeViewModel
    {
        //public List<BlogViewModel> LastestBlogs { get; set; }
        //public List<SlideViewModel> HomeSlides { get; set; }
        //public List<ProductViewModel> HotProducts { get; set; }
        //public List<ProductViewModel> TopSellProducts { get; set; }

        public Task<List<ProductViewModel>> HomeCategories { set; get; }

        public Task<List<ProductViewModel>> RelatedProducts { get; set; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}
