using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels.CShop
{
    public class ShoppingCartViewModel
    {      
        public ProductViewModel Product { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public AttributeOptionValueViewModel Color { get; set; }

        public AttributeOptionValueViewModel Size { get; set; }
    }
}
