using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.WebErp.Extensions;
using NiTiAPI.WebErp.Filters;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class AppRoleController : BaseController
    {
        private readonly IRoleRepository _role;

        public AppRoleController(IRoleRepository role)
        {
            _role = role;            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
        {
            var model = await _role.GetPaging(keyword, cororationId, pageIndex, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoporationId(int cororationId)
        {
            var model = await _role.GetPaging("%", cororationId, 1, 1000);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListRole()
        {
            var model = await _role.GetListRole();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleId(Guid id)
        {
            var model = await _role.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, ActionCode.CREATE)]
        public async Task<IActionResult> CreateRole(RoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {                
                roleVm.CreateDate = DateTime.Now;               
                var role = await _role.Create(roleVm);
                return new OkObjectResult(role);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdateRole(RoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {                
                roleVm.UpdateDate = DateTime.Now;
                var role = await _role.Update(roleVm);
                return new OkObjectResult(role);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, ActionCode.DELETE)]
        public async Task<IActionResult> DeleteRole(Guid Id, string userName)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {         
                var role = await _role.Delete(Id, userName);
                return new OkObjectResult(role);
            }
        }

        #region Function Permission Action       

        [HttpPost]
        //[ClaimRequirement(FunctionCode.SYSTEM_ROLE, ActionCode.CREATE)]
        public async Task<IActionResult> SavePermission(FunctionPermisionViewModel functionPermissionVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                functionPermissionVm.CreateDate = DateTime.Now;
                var functionPermission = await _role.FunctionPermissionCreateXML(functionPermissionVm);
                return new OkObjectResult(functionPermission);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListFunPer(Guid roleId)
        {          
            var model = await _role.GetListFuntionPermissionByRole(roleId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _role.GetListFuntionPermission();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFunPer()
        {
            var model = await _role.GetListFuntionPermission();
            var rootFunctions = model.Where(c => c.ParentId == null);
            //var rootFunctions = model;
            var items = new List<FunctionPermisionViewModel>();
            foreach (var function in rootFunctions)
            {
                //add the parent category to the item list
                items.Add(function);
                //now get all its children (separate Category in case you need recursion)
                GetByParentId(model.ToList(), function, items);
            }
            return new ObjectResult(items);
        }

        private void GetByParentId(IEnumerable<FunctionPermisionViewModel> allFunctions, FunctionPermisionViewModel parent, IList<FunctionPermisionViewModel> items)
        {
            var functionsEntities = allFunctions as FunctionPermisionViewModel[] ?? allFunctions.ToArray();
            //var subFunctions = functionsEntities.Where(c => c.ParentId == parent.Id);
            var subFunctions = functionsEntities.Where(c => c.ParentId == parent.FunctionId);
            foreach (var cat in subFunctions)
            {
                //add this category
                items.Add(cat);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }

        #endregion


    }
}