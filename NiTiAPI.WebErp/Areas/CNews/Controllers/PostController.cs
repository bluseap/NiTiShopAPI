using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels.CNews;

namespace NiTiAPI.WebErp.Areas.CNews.Controllers
{
    public class PostController : BaseController
    {
        private readonly ICategoryNewsRepository _categoryNews;
        private readonly IPostRepository _post;
        IConfiguration _configuration;

        public PostController(ICategoryNewsRepository categoryNews, IPostRepository post,
            IConfiguration configuration)
        {
            _categoryNews = categoryNews;
            _post = post;
            _configuration = configuration;
        }       

        public IActionResult Index(string id, int catelogyNewsId, string keyword, int pageSize, string sortBy, int page = 1)
        {
            HttpContext.Session.Clear();
            ViewData["CorporationName"] = id; // corporationName convert to corporationId = 1

            ViewData["CorporationId"] = id;
            ViewData["catelogyNewsId"] = catelogyNewsId;

            //ViewData["BodyClass"] = "cms-index-index cms-home-page";            
            if (id != null)
            {
                HttpContext.Session.SetString("corprationName", id);
            }
            else
            {
                HttpContext.Session.SetString("corprationName", "");
            }

            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var postNews = new SearchResultCNewsViewModel();

            if (pageSize == 0)
                pageSize = _configuration.GetValue<int>("PageSize");

            if (keyword == null)
                keyword = "0";

            postNews.PageSize = pageSize;
            postNews.SortType = sortBy != null ? "1" : "0";
            var postVm = _post.GetPaging("%", culture, 1, catelogyNewsId, page, pageSize);
            postNews.Data = postVm;
            postNews.Keyword = keyword;

            return View(postNews);
        }

        public IActionResult Details(string id, string postId)
        {
            HttpContext.Session.Clear();
            ViewData["CorporationName"] = id; // corporationName convert to corporationId = 1

            ViewData["CorporationId"] = id;
            ViewData["postId"] = postId;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCoporationId(int cororationId)
        {
            var model = await _categoryNews.GetPaging("%", cororationId, 1, 1000);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(string keyword, string culture, int corporationId, int categoryNewsId,
            int pageIndex, int pageSize)
        {
            var model = await _post.GetPaging(keyword, culture, corporationId, categoryNewsId, pageIndex, pageSize);
            return new OkObjectResult(model);
        }
        

    }
}