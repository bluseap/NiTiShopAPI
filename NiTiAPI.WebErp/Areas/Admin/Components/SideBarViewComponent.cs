using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using NiTiAPI.Utilities.Constants;
using NiTiAPI.WebErp.Extensions;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;

namespace NiTiAPI.WebErp.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionRepository _functionRepository;

        public SideBarViewComponent(IFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string filter = "%";

            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;

            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionRepository.GetAll(filter);
            }
            else
            {
                //TODO: Get by permission
                functions = await _functionRepository.GetAll(filter);
            }
            return View(functions);
        }

    }
}