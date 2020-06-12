using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels.CShop;

namespace NiTiAPI.WebErp.Areas.CShop.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactRepository _contactRepository;        

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;           
        }

        public IActionResult Index()
        {
            ViewData["CorporationName"] = 1;
            HttpContext.Session.SetString("corprationName", "1");
           
            var contact = _contactRepository.GetByCorId(1);
            var model = new ContactPageViewModel { Contact = contact.Result };
            return View(model);

            //ViewData["CorporationName"] = id;
            //if (id != null)
            //{
            //    HttpContext.Session.SetString("corprationName", id);
            //}
            //else
            //{
            //    HttpContext.Session.SetString("corprationName", "");
            //}
            //var contact = _contactRepository.GetByCorName(id);
            //var model = new ContactPageViewModel { Contact = contact.Result };
            //return View(model);
        }

    }
}