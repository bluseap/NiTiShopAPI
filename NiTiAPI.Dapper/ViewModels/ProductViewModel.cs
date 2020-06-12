using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.ViewModels
{
    public class ProductViewModel
    {
        public ICollection<ProductViewModel> Products { set; get; }

        public long Id { get; set; }

        public long ProductId { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int ParentId { get; set; }        

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }       

        public decimal DiscountPrice { get; set; }

        public string ImageUrl { get; set; }

        public string ImageList { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public bool HomeFlag { get; set; }

        public bool HotFlag { get; set; }

        public int RateTotal { get; set; }

        public int RateCount { get; set; }


        public string LanguageId { get; set; }

        public string Contents { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SeoDescription { get; set; }

        public string SeoAlias { get; set; }

        public string SeoTitle { get; set; }

        public string SeoKeyword { get; set; }

        public string SeoTags { get; set; }


        public string AttributeValueText { get; set; }


        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }
}
