using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.WebErp.Filters;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class CorporationController : BaseController
    {
        private readonly ICorporationRepository _corporation;

        public CorporationController(ICorporationRepository corporation)
        {
            _corporation = corporation;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]        
        public async Task<IActionResult> GetListCorporations()
        {
            var model = await _corporation.GetListCorporations();
            return new OkObjectResult(model);
        }

        [HttpGet]       
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _corporation.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetByOrderId(long orderId)
        {
            var model = await _corporation.GetByOrderId(orderId);
            return new OkObjectResult(model);
        }




    }
}