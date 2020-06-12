using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class UserOnlineViewModel
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public int UserOnline { get; set; }

        public int TotalUser { get; set; }

        public string Note { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }
}
