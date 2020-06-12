using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels.CShop;

namespace NiTiAPI.WebErp.Areas.CShop.Controllers
{   
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IProductRepository _productRepository;

        public HomeController(IStringLocalizer<HomeController> localizer, IProductRepository productRepository)
        {
            _localizer = localizer;
            _productRepository = productRepository;
        }
           
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            ViewData["CorporationName"] = 1; // corporationName convert to corporationId = 1
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
           
            //if (id != null)
            //{
            //    HttpContext.Session.SetString("corprationName", id);
            //}
            //else
            //{
            //    HttpContext.Session.SetString("corprationName", "");
            //}

            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var homeVm = new ClientHomeViewModel();

            homeVm.HomeCategories = _productRepository.GetListProductCatelogCorId1(1, culture);
            //homeVm.HotProducts = _productService.GetHotProduct(5);
            //homeVm.TopSellProducts = _productService.GetLastest(10);
            //homeVm.LastestBlogs = _blogService.GetLastest(5);
            //homeVm.HomeSlides = _commonService.GetSlides("top");

            homeVm.RelatedProducts = _productRepository.GetListProductCorTopId2(1, culture, 9);

            return View(homeVm);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    

    }
}