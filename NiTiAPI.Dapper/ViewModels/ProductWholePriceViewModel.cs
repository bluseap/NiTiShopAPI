using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class ProductWholePriceViewModel
    {
        public int Id { get; set; }

        public long ProductId { get; set; }

        public int FromQuantity { get; set; }

        public decimal Price { get; set; }

        public int ToQuantity { get; set; }


        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }
}
