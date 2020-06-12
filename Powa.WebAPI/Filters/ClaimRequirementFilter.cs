using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Powa.WebAPI.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly FunctionCode _function;
        private readonly ActionCode _action;

        public ClaimRequirementFilter(FunctionCode function, ActionCode action)
        {
            _function = function;
            _action = action;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            var permissionsClaim = context.HttpContext.User.Claims.SingleOrDefault(c => c.Type == NiTiAPI.Utilities.Constants.SystemConstants.UserClaim.Permissions);
            if (permissionsClaim != null)
            {
                var permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
                var functionArr = _function.ToString().Split("_");
                string functionId = string.Join(".", functionArr);
                if (!permissions.Contains(functionId + "_" + _action))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
       
    }
}
