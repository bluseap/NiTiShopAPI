using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels.CNews;

namespace NiTiAPI.WebErp.Areas.CNews.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryNewsRepository _categoryNewsRepository;

        public HomeController(IStringLocalizer<HomeController> localizer, IPostRepository postRepository,
            ICategoryNewsRepository categoryNewsRepository)
        {
            _localizer = localizer;
            _postRepository = postRepository;
            _categoryNewsRepository = categoryNewsRepository;
        }

        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            ViewData["CorporationName"] = 1; // corporationName convert to corporationId = 1
            //ViewData["BodyClass"] = "cms-index-index cms-home-page";            

            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var homeVm = new HomeCNewsViewModel();
            homeVm.PostHomeCategoryNews = _categoryNewsRepository.GetListHomeCateByCorLangId(1, culture);

            homeVm.PostIntro = _postRepository.GetPaging("", culture, 1, 1, 1, 1).Result.Items.ToList();
            homeVm.PostProduct = _postRepository.GetPaging("", culture, 1, 3, 1, 5).Result.Items.ToList();
            homeVm.PostProductSilderRight = _postRepository.GetPaging("", culture, 1, 3, 1, 2).Result.Items.ToList();
            homeVm.PostEvent = _postRepository.GetPaging("", culture, 1, 2, 1, 5).Result.Items.ToList();
            homeVm.PostEventSilderRight = _postRepository.GetPaging("", culture, 1, 2, 1, 2).Result.Items.ToList();
            homeVm.PostCustomerInfo = _postRepository.GetPaging("", culture, 1, 4, 1, 5).Result.Items.ToList();

            return View(homeVm);
            
        }

        //[HttpPost]
        //public IActionResult SetLanguage(string culture, string returnUrl)
        //{
        //    Response.Cookies.Append(
        //        CookieRequestCultureProvider.DefaultCookieName,
        //        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        //        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        //    );

        //    return LocalRedirect(returnUrl);
        //}


    }
}