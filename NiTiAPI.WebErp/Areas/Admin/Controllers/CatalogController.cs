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
    public class CatalogController : BaseController
    {
        private readonly ICategoriesRepository _category;

        public CatalogController(ICategoriesRepository category)
        {
            _category = category;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCoporationId(int cororationId)
        {
            var model = await _category.GetPaging("%", cororationId, 1, 1000);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListCategory()
        {
            var model = await _category.GetListCategory();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryId(int Id)
        {
            var model = await _category.GetById(Id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_CATALOG, ActionCode.CREATE)]
        public async Task<IActionResult> Create(CategoriesViewModel CateVm)
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
                var cate = await _category.Create(CateVm);
                return new OkObjectResult(cate);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_CATALOG, ActionCode.UPDATE)]
        public async Task<IActionResult> Update(CategoriesViewModel CateVm)
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
                var cate = await _category.Update(CateVm);
                return new OkObjectResult(cate);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_CATALOG, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdateParent(int fromParent, int toParent, int parameter, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var cate = await _category.UpdateParent(fromParent, toParent, parameter, username);
                return new OkObjectResult(cate);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_CATALOG, ActionCode.DELETE)]
        public async Task<IActionResult> Delete(int Id, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var cate = await _category.Delete(Id, username);
                return new OkObjectResult(cate);
            }
        }

    }
}