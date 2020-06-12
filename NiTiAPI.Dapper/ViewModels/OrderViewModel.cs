using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public int CustomerId { get; set; }
       
        [Required(ErrorMessage = "CustomerName")]
        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "CustomerAddress")]
        [Display(Name = "Customer address")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "CustomerEmail")]
        [Display(Name = "Customer email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "CustomerPhone")]
        [Display(Name = "Customer phone")]
        public string CustomerPhone { get; set; }

        public string CustomerNote { get; set; }


        public long ProductId { get; set; }

        public int AttributeOptionValueIdColor { get; set; }

        public int AttributeOptionValueIdSize { get; set; }

        public float SellPrice { get; set; }

        public int Quantity { get; set; }


        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }



    }
}
