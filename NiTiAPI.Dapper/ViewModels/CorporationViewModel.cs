using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class CorporationViewModel
    {
        public int Id { set; get; }
      
        public int InsertRole { get; set; }

        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string Name { get; set; }

        public string Aliases { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public string ImageLogo { get; set; }

        public string MobileNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string TaxNumber { get; set; }

        public string WebName { get; set; }

        public string ParentId { set; get; }

        public string Description { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }



    }
}
