using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class FeedBackViewModel
    {
        public int Id { get; set; }

        public int CorporationId { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }
}
