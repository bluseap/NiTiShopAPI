using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels.CShop;

namespace NiTiAPI.WebErp.Areas.CShop.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IProductRepository _productRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IAttributeOptionValueRepository _attributeOption;
        IConfiguration _configuration;

        public ProductController(IStringLocalizer<HomeController> localizer, IProductRepository productRepository,
            ICategoriesRepository categoriesRepository, IProductImagesRepository productImagesRepository,
            IAttributeOptionValueRepository attributeOption,
            IConfiguration configuration )
        {
            _localizer = localizer;
            _productRepository = productRepository;
            _categoriesRepository = categoriesRepository;
            _productImagesRepository = productImagesRepository;
            _attributeOption = attributeOption;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewData["CorporationName"] = 1;
            HttpContext.Session.SetString("corprationName", "1");
            
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            return View();
            //ViewData["CorporationName"] = id;
            //if (id != null)
            //{
            //    HttpContext.Session.SetString("corprationName", id);
            //}
            //else
            //{
            //    HttpContext.Session.SetString("corprationName", "");
            //}
            //var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;     
            //return View();
        }

        public IActionResult Details(string id, string productId)
        {
            //HttpContext.Session.Clear();
            ViewData["BodyClass"] = "product-page";
            ViewData["CorporationName"] = 1;
            ViewData["ProductId"] = productId;

            HttpContext.Session.SetString("corprationName", "1");
            //if (id != null)
            //{
            //    HttpContext.Session.SetString("corprationName", id);               
            //}
            //else
            //{
            //    HttpContext.Session.SetString("corprationName", "");               
            //}

            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var model = new ProductDetailViewModel();

            if (productId != null)
            {
                model.Product = _productRepository.GetById(Convert.ToInt64(productId), culture);
                var product = model.Product.Result;
                if (product != null)
                {
                    model.Category = _categoriesRepository.GetById(product.CategoryId);
                }
                model.ProductImages = _productImagesRepository.GetListProductImages(Convert.ToInt64(productId));

                //model.Colors = _billService.GetColors().Select(x => new SelectListItem()
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString()
                //}).ToList();
                //model.Sizes = _billService.GetSizes().Select(x => new SelectListItem()
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString()
                //}).ToList();

                //model.RelatedProducts = _productRepository.GetListProductCorNameTop(id, culture, 9);
                model.RelatedProducts = _productRepository.GetListProductCorTopId2(1, culture, 9);

                model.AttributeSizes = _attributeOption.GetListByAttributeSize(Convert.ToInt64(productId), culture);
            }

            //model.UpsellProducts = _productService.GetUpsellProducts(6);
            //model.Tags = _productService.GetProductTags(productId);

            return View(model);
        }

        public IActionResult Search(string id, string catelogyId, string keyword, int pageSize, string sortBy, int page = 1)
        {
            //HttpContext.Session.Clear();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            ViewData["CorporationName"] = 1;
            HttpContext.Session.SetString("corprationName", "1");
            //if (id != null)
            //{
            //    HttpContext.Session.SetString("corprationName", id);
            //}
            //else
            //{
            //    HttpContext.Session.SetString("corprationName", "");
            //}

            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var catalog = new SearchResultViewModel();
           
            if (pageSize == 0)
                pageSize = _configuration.GetValue<int>("PageSize");

            if (keyword == null)
                keyword = "0";

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy != null ? "1" : "0";
            var productVm = _productRepository.GetAllPagingProductCateCorId(1, catelogyId, culture, keyword, page, pageSize);
            catalog.Data = productVm;
            catalog.Keyword = keyword;

            return View(catalog);
        }

        [HttpGet]
        public async Task<IActionResult> GetListCategory()
        {
            //var corporationName = HttpContext.Session.GetString("corprationName");
            var model = await _categoriesRepository.GetListCateByCorId(1);
            return new OkObjectResult(model);

            //var corporationName = HttpContext.Session.GetString("corprationName");
            //var model = await _categoriesRepository.GetListCateByCor(corporationName);
            //return new OkObjectResult(model);            
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

        [HttpGet]
        public async Task<IActionResult> GetListAttributeAll()
        {
            var model = await _attributeOption.GetListAll();
            return new OkObjectResult(model);
        }




    }
}