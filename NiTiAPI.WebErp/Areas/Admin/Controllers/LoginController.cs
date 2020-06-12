
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Utilities.Dtos;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IAppUserLoginRepository _appuserloginrepository;
       

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ILogger<LoginController> logger, IAppUserLoginRepository appuserloginrepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appuserloginrepository = appuserloginrepository;           
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(AppUser model)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (ModelState.IsValid)
            {               

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.PasswordHash, false, true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    
                    return new OkObjectResult(new GenericResult(true));
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new ObjectResult(new GenericResult(false, "User account locked."));
                }
                else
                {
                    return new ObjectResult(new GenericResult(false, "Login failed."));
                }
            }
           
            // If we got this far, something failed, redisplay form
            return new ObjectResult(new GenericResult(false, model));
        }        


    }
}