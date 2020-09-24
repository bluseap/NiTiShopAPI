
using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
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

                    CountUserLogin(model.Email);

                    return new OkObjectResult(new GenericResult(true));
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    CountUserLogin(model.Email);
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

        public void CountUserLogin(string userNameId)
        {
            string ipString = HttpContext.Connection.RemoteIpAddress.ToString(); // LoginIpAddress

            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());

            //var ip = heserver.AddressList[2].ToString();
            var nameComputer = heserver.HostName.ToString(); // LoginNameIp
            var localIp6 = heserver.AddressList[0] != null ? heserver.AddressList[0].ToString() : "";
            var temIp6 = heserver.AddressList[1] != null ? heserver.AddressList[1].ToString() : "";
            var ip6Address = "";
            var ipComputer = ipString;//heserver.AddressList[3].ToString(); // LoginIp


            var appuserloginVm = new AppUserLoginViewModel();

            //var username = User.GetSpecificClaim("UserName");
            var username = userNameId;

            appuserloginVm.UserName = username;

            appuserloginVm.LoginIpAddress = ipString;
            appuserloginVm.LoginIp = ipComputer;
            appuserloginVm.LoginNameIp = nameComputer;
            appuserloginVm.LoginIp6Address = ip6Address;
            appuserloginVm.LoginLocalIp6Adress = localIp6;
            appuserloginVm.LoginMacIp = temIp6;

            appuserloginVm.CreateDate = DateTime.Now;
            appuserloginVm.CreateBy = username;

            var model = _appuserloginrepository.AppUserLoginAUD(appuserloginVm, "InAppUserLogin");
           
        }


    }
}