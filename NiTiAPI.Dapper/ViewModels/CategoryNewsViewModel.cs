using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class CategoryNewsViewModel
    {
        public int Id { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SeoAlias { get; set; }

        public string SeoTitle { get; set; }

        public string SeoKeyword { get; set; }

        public string SeoDescription { get; set; }

        public string SeoTags { get; set; }


        public int ParentId { get; set; }

        public int SortOrder { get; set; }

        public bool ShowInMenu { get; set; }

        public bool ShowInHome { get; set; }

        public string Thumbnail { get; set; }

        public bool IsActive { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }       

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }
}
