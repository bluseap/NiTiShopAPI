using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Repositories.Interfaces;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class AttributeOptionValueController : BaseController
    {
        private readonly IAttributeOptionValueRepository _attributeOption;

        public AttributeOptionValueController(IAttributeOptionValueRepository attributeOption)
        {
            _attributeOption = attributeOption;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListAttribute(int attributeId, string language)
        {
            var model = await _attributeOption.GetListByAttribute(attributeId, language);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAttributeSize(long productId, string language)
        {
            var model = await _attributeOption.GetListByAttributeSize(productId, language);
            return new OkObjectResult(model);
        }


    }
}