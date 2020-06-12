using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.WebErp.Filters;


namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class CategoryNewsController : BaseController
    {
        private readonly ICategoryNewsRepository _categoryNews;

        public CategoryNewsController(ICategoryNewsRepository categoryNews)
        {
            _categoryNews = categoryNews;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCoporationId(int cororationId)
        {
            var model = await _categoryNews.GetPaging("%", cororationId, 1, 1000);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListCategory()
        {
            var model = await _categoryNews.GetListCategory();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryId(int Id)
        {
            var model = await _categoryNews.GetById(Id);
            return new OkObjectResult(model);
        }       

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_CATALOG, ActionCode.CREATE)]
        public async Task<IActionResult> Create(CategoryNewsViewModel CateVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                CateVm.IsActive = true;
                CateVm.CreateDate = DateTime.Now;
                var cate = await _categoryNews.Create(CateVm);
                return new OkObjectResult(cate);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_CATALOG, ActionCode.UPDATE)]
        public async Task<IActionResult> Update(CategoryNewsViewModel CateVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                CateVm.IsActive = true;
                CateVm.UpdateDate = DateTime.Now;
                var cate = await _categoryNews.Update(CateVm);
                return new OkObjectResult(cate);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_CATALOG, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdateParent(int fromParent, int toParent, int parameter, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var cate = await _categoryNews.UpdateParent(fromParent, toParent, parameter, username);
                return new OkObjectResult(cate);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_CATALOG, ActionCode.DELETE)]
        public async Task<IActionResult> Delete(int Id, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var cate = await _categoryNews.Delete(Id, username);
                return new OkObjectResult(cate);
            }
        }

    }
}