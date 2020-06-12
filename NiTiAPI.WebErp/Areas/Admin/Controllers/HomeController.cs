using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Localization;
using NiTiAPI.Dapper.Repositories.Interfaces;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IUserOnlineRepository _userOnlineRepository;

        public HomeController( IStringLocalizer<HomeController> localizer, 
            IUserOnlineRepository userOnlineRepository)
        {          
            _localizer = localizer;
            _userOnlineRepository = userOnlineRepository;
        }

        public IActionResult Index()
        {
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            return View();
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

        [HttpPost]       
        public async Task<IActionResult> AddUserOnline(int corporationId, int CountUser, string notes)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {            

                var userOnline = await _userOnlineRepository.AddUserOnline(corporationId, CountUser, notes);
                return new OkObjectResult(userOnline);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalUserOnline(int coporationId)
        {
            var model = await _userOnlineRepository.GetListCorporation(coporationId);
            var result = model.Max(p => p.TotalUser);
            return new OkObjectResult(result);
        }

    }
}