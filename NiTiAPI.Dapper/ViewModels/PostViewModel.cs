using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public int CategoryNewsId { get; set; }

        public string CategoryNewsName { get; set; }

        public string Thumbnail { get; set; }

        public string Image { get; set; }

        public string ImageCaption { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Source { get; set; }

        public int Status { get; set; }

        public DateTime HotDate { get; set; }

        public DateTime NewDate { get; set; }

        public bool  IsActive { get; set; }

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }


        public string LanguageId { get; set; }

        public string ListImageXML { get; set; }

        public string NameLanguage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }                

        public string SeoAlias { get; set; }

        public string SeoTitle { get; set; }

        public string SeoMetaKeywords { get; set; }

        public string SeoMetaDescription { get; set; }

        public string SeoTags { get; set; }


    }
}
