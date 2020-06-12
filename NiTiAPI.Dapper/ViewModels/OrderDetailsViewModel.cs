using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int SortNumber { get; set; }

        public long OrderId { get; set; }

        public string CustomerName { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int AttributeOptionValueIdColor { get; set; }

        public int AttributeOptionValueIdSize { get; set; }

        public decimal OriginalPrice { get; set; }

        public float Price { get; set; }

        public decimal DiscountPrice { get; set; }

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
