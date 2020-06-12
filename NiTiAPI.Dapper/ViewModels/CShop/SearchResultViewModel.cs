using Microsoft.AspNetCore.Mvc.Rendering;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.ViewModels.CShop
{
    public class SearchResultViewModel
    {
        public Task<PagedResult<ProductViewModel>> Data { get; set; }

        public CategoriesViewModel Category { set; get; }

        public string SortType { set; get; }

        public int? PageSize { set; get; }

        public string Keyword { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "lastest",Text = "Lastest"},
            new SelectListItem(){Value = "price",Text = "Price"},
            new SelectListItem(){Value = "name",Text = "Name"},
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "24",Text = "24"},
            new SelectListItem(){Value = "48",Text = "48"},
        };

    }
}
