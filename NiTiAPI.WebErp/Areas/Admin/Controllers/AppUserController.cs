using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using NiTiAPI.WebErp.Filters;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class AppUserController : BaseController
    {
        private readonly IUserRepository _user;
        private readonly IUserRolesRepository _userrolesRepository;

        public AppUserController(IUserRepository user, IUserRolesRepository userrolesRepository)
        {
            _user = user;
            _userrolesRepository = userrolesRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
        {
            var model = await _user.GetPaging(keyword, cororationId, pageIndex, pageSize);
            return new OkObjectResult(model);
        }        

        [HttpGet]
        public async Task<IActionResult> GetUserId(Guid id)
        {
            var model = await _user.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, ActionCode.CREATE)]
        public async Task<IActionResult> SaveUser(UserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                userVm.Id = Guid.NewGuid();
                userVm.Status = 1;
                userVm.Active = true;
                userVm.SortOrder = 1; 
                userVm.CreateDate = DateTime.Now;

                var user = await _user.CreateUser(userVm);
                return new OkObjectResult(user);                
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdateUser(UserViewModel userVm, string Roles)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {                
                userVm.Status = 1;                
                userVm.SortOrder = 1;
                userVm.UpdateDate = DateTime.Now;

                var user = await _user.UpdateUser(userVm, Roles);
                return new OkObjectResult(userVm);
                
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdateUserPass(UserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {                             
                userVm.UpdateDate = DateTime.Now;

                var user = await _user.UpdateUserPass(userVm);
                return new OkObjectResult(user);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, ActionCode.DELETE)]
        public async Task<IActionResult> DeleteUser(Guid Id, string userName)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var user = await _user.Delete(Id, userName);
                return new OkObjectResult(user);
            }
        }

        #region List User Role

        [HttpGet]
        public async Task<IActionResult> GetListUserRole(Guid userId)
        {
            var model = await _userrolesRepository.GetListUserRole(userId);
            return new OkObjectResult(model);
        }

        #endregion

    }
}