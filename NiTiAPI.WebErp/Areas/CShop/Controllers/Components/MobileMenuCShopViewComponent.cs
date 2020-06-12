using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NiTiAPI.WebErp.Areas.CShop.Controllers.Components
{
    public class MobileMenuCShopViewComponent : ViewComponent
    {        

        private IProductRepository _productRepository;

        public MobileMenuCShopViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = _productRepository.GetListProductCatelogCorId1(1, "vi-VN");
            return View(product);

            //var corporationName = HttpContext.Session.GetString("corprationName");
            //if (corporationName != null && corporationName != "favicon.ico")
            //{
            //    var product = _productRepository.GetListProductCatelogCorName(corporationName, "vi-VN");
            //    return View(product);
            //}
            //else
            //{
            //    return View();
            //}
        }

    }
}
