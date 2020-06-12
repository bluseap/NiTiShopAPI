using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.ViewModels.CNews
{
    public class HomeCNewsViewModel
    {
        public List<CategoryNewsViewModel> PostHomeCategoryNews { set; get; }

        public List<PostViewModel> PostIntro { set; get; }
        public List<PostViewModel> PostProduct { set; get; }
        public List<PostViewModel> PostProductSilderRight { set; get; }
        public List<PostViewModel> PostEvent { set; get; }
        public List<PostViewModel> PostEventSilderRight { set; get; }
        public List<PostViewModel> PostCustomerInfo { set; get; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }

    }
}
