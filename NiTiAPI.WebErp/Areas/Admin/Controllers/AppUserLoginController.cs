using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class AppUserLoginController : BaseController
    {        
        private readonly IAppUserLoginRepository _appuserloginrepository;

        public AppUserLoginController(IAppUserLoginRepository appuserloginrepository)
        {
            _appuserloginrepository = appuserloginrepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void CountUserLogin(string userNameId, string statuscontent)
        {
            string ipString = HttpContext.Connection.RemoteIpAddress.ToString(); // LoginIpAddress

            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());

            //var ip = heserver.AddressList[2].ToString();
            var nameComputer = heserver.HostName.ToString(); // LoginNameIp
            var localIp6 = heserver.AddressList[0] != null ? heserver.AddressList[0].ToString() : "";
            var temIp6 = heserver.AddressList[1] != null ? heserver.AddressList[1].ToString() : "";
            var ip6Address = "";
            var ipComputer = ipString;//heserver.AddressList[3].ToString(); // LoginIp
            var appuserloginVm = new AppUserLogin();
            var username = userNameId;

            appuserloginVm.UserName = username;
            appuserloginVm.LoginIpAddress = ipString;
            appuserloginVm.LoginIp = ipComputer;
            appuserloginVm.LoginNameIp = nameComputer;
            appuserloginVm.LoginIp6Address = ip6Address;
            appuserloginVm.LoginLocalIp6Adress = localIp6;
            appuserloginVm.LoginMacIp = temIp6;
            appuserloginVm.StatusContent = statuscontent;
            appuserloginVm.CreateDate = DateTime.Now;
            appuserloginVm.CreateBy = username;

            var model = _appuserloginrepository.Create(appuserloginVm);
        }

    }
}