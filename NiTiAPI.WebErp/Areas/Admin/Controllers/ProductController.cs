using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.WebErp.Filters;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductRepository _product;
        private readonly IProductImagesRepository _productImages;
        private readonly IProductQuantitiesRepository _productQuantities;
        private readonly IProductWholePriceRepository _productWholePrice;
        private readonly IAttributeRepository _attributeRepository;

        public ProductController(IProductRepository product, IProductImagesRepository productImages,
            IProductQuantitiesRepository productQuantities, IProductWholePriceRepository productWholePrice,
            IAttributeRepository attributeRepository)
        {
            _product = product;
            _productImages = productImages;
            _productQuantities = productQuantities;
            _productWholePrice = productWholePrice;
            _attributeRepository = attributeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductId(long id, string culture)
        {
            var model = await _product.GetById(id, culture);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(string keyword, int corporationId, int categoryId,
            int pageIndex, int pageSize, string culture)
        {
            var model = await _product.GetPagingProduct(keyword, corporationId, categoryId, pageIndex, pageSize, culture);
            return new OkObjectResult(model);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.CREATE)]
        public async Task<IActionResult> CreateProduct(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.CreateDate = DateTime.Now;
                var product = await _product.CreateProduct(productVm, "vi-VN");
                return new OkObjectResult(product);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdateProduct(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.UpdateDate = DateTime.Now;
                var product = await _product.UpdateProduct(productVm, "vi-VN");
                return new OkObjectResult(product);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.DELETE)]
        public async Task<IActionResult> DeleteProduct(long Id, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var role = await _product.DeleteProduct(Id, username);
                return new OkObjectResult(role);
            }
        }

        #region Product Images

        [HttpGet]
        public async Task<IActionResult> GetProductImages(long productId)
        {
            var model = await _productImages.GetListProductImages(productId);
            return new OkObjectResult(model);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.CREATE)]
        public async Task<IActionResult> SaveImages(long productId, string images, string username)
        {
            var productImages = await _productImages.ProductImages(productId, images, username);            
            return new OkObjectResult(productImages);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.DELETE)]
        public async Task<IActionResult> DeleteImage(long productImageId, string username)
        {
            var productImages = await _productImages.DeleteImage(productImageId, username);
            return new OkObjectResult(productImages);
        }

        #endregion

        #region Product Quantities

        [HttpGet]
        public async Task<IActionResult> GetProductQuantities(long productId)
        {
            var productQuantities = await _productQuantities.GetListProductQuantities(productId);
            return new OkObjectResult(productQuantities);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.CREATE)]
        public async Task<IActionResult> SaveQuantities(string productQuantiesXML, string username, string languageId)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {                
                var productQuantities = await _productQuantities.CreateProductQuantities(productQuantiesXML, username, languageId);
                return new OkObjectResult(productQuantities);
            }            
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.DELETE)]
        public async Task<IActionResult> DeleteQuantities(long productQuantitiesId, string username)
        {
            var productQuantities = await _productQuantities.DeleteQuantities(productQuantitiesId, username);
            return new OkObjectResult(productQuantities);
        }

        #endregion

        #region Product Whole Price

        [HttpGet]
        public async Task<IActionResult> GetProductWholePrice(long productId)
        {
            var productWholePrice = await _productWholePrice.GetListProductWholePrice(productId);
            return new OkObjectResult(productWholePrice);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.SALES_PRODUCT, ActionCode.CREATE)]
        public async Task<IActionResult> SaveWholePrice(string productWholePriceXML, string username, string languageId)
        {            
            var productWholePrice = await _productWholePrice.CreateProductWholePrice(productWholePriceXML, username, languageId);
            return new OkObjectResult(productWholePrice);           
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributeSize(string codeSize, string languageId)
        {
            var model = await _attributeRepository.GetByCodeSize(codeSize, languageId);
            return new OkObjectResult(model);
        }

        #endregion

    }
}