using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NiTiAPI.WebErp.Areas.CNews.Controllers.Components
{
    public class CategoryMenuCNewsViewComponent : ViewComponent
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private ICategoryNewsRepository _categoryNewsRepository;

        public CategoryMenuCNewsViewComponent(IStringLocalizer<HomeController> localizer, ICategoryNewsRepository categoryNewsRepository)
        {
            _localizer = localizer;
            _categoryNewsRepository = categoryNewsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var product = _categoryNewsRepository.GetListCateByCorId(1);
            return View(product);           
        }

    }
}
