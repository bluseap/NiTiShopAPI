using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels.CShop;
using NiTiAPI.Utilities.Constants;
using NiTiAPI.WebErp.Extensions;

namespace NiTiAPI.WebErp.Areas.CShop.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IAttributeOptionValueRepository _attributeOptionValueRepository;
        private readonly IOrderRepository _orderRepository;

        public CartController(IProductRepository productRepository, 
            IAttributeOptionValueRepository attributeOptionValueRepository,
            IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _attributeOptionValueRepository = attributeOptionValueRepository;
            _orderRepository = orderRepository;
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
      
        public IActionResult Order(string returnUrl = null)
        {
            ViewData["CorporationName"] = 1;
            HttpContext.Session.SetString("corprationName", "1");       
            
            ViewData["ReturnUrl"] = returnUrl;
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
            //ViewData["ReturnUrl"] = returnUrl;
            //var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            //return View();
        }

        public IActionResult OrderInfo(string returnUrl = null)
        {
            ViewData["CorporationName"] = 1;
            HttpContext.Session.SetString("corprationName", "1");
           
            ViewData["ReturnUrl"] = returnUrl;
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
            //ViewData["ReturnUrl"] = returnUrl;
            //var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            //return View();
        }
       
        public async Task<IActionResult> BeginOrder(string corporationname, string customerName, string customerEmail,
            string customerPhone, string customerAddress)
        {
            string xml = "";
            xml = xml + "<tables>";            

            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
          
            if (session != null)
            {   
                for (int i = 0; i < session.Count; i++)
                {
                    var listfield = session[i];
                    xml += "<items>";
                    //xml += "<CorporationName>" + corporationname.ToUpper() + "</CorporationName>";
                    xml += "<CorporationName>" + "1" + "</CorporationName>";
                    xml += "<CustomerName>" + customerName + "</CustomerName>";
                    xml += "<CustomerEmail>" + customerEmail + "</CustomerEmail>";
                    xml += "<CustomerPhone>" + customerPhone + "</CustomerPhone>";
                    xml += "<CustomerAddress>" + customerAddress + "</CustomerAddress>";
                    xml += "<ProductId>" + listfield.Product.Id + "</ProductId>";
                    xml += "<Quantity>" + listfield.Quantity + "</Quantity>";
                    xml += "<SizeId>" + listfield.Size.Id + "</SizeId>";
                    xml += "<ColorId>" + listfield.Color.Id + "</ColorId>";
                    xml += "</items>";
                }                      
            }

            xml = xml + "</tables>";

            //var orderXML = await _orderRepository.CreateOrder(xml, "GuidId");
            var orderXML = await _orderRepository.CreateOrderCorpoId(xml, "GuidId");
            return new OkObjectResult(orderXML);            
        }

        [HttpGet]
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }
       
        public IActionResult AddToCart(long productId, int quantity, int color, int size)
        {

            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            //Get product detail
            var product = _productRepository.GetById(productId, culture);

            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;

                //Check exist with item product id
                if (session.Any(x => x.Product.Id == productId && x.Size.Id == size && x.Color.Id == color))
                {
                    foreach (var item in session)
                    {
                        //Update quantity for product if match product id
                        if (item.Product.Id == productId && item.Size.Id == size && item.Color.Id == color)
                        {
                            item.Quantity += quantity;
                            item.Price = product.Result.DiscountPrice == 0 ? product.Result.Price : product.Result.DiscountPrice;
                            hasChanged = true;
                        }                        
                    }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product.Result,
                        Quantity = quantity,
                        Color = _attributeOptionValueRepository.GetById(color).Result, 
                        Size = _attributeOptionValueRepository.GetById(size).Result,
                        Price = product.Result.DiscountPrice == 0 ? product.Result.Price : product.Result.DiscountPrice
                });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product.Result,
                    Quantity = quantity,
                    Color = _attributeOptionValueRepository.GetById(color).Result,
                    Size = _attributeOptionValueRepository.GetById(size).Result,
                    Price = product.Result.DiscountPrice == 0 ? product.Result.Price : product.Result.DiscountPrice
            });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(productId);
        }

        public IActionResult RemoveFromCart(long productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        public IActionResult UpdateCart(long productId, int quantity, int color, int size)
        {
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productRepository.GetById(productId, culture);

                        if (color == 0 && size == 0)
                        {
                            item.Product = product.Result;
                            //item.Size = _attributeOptionValueRepository.GetById(size).Result;
                            //item.Color = _attributeOptionValueRepository.GetById(color).Result;
                            item.Quantity = quantity;
                            item.Price = product.Result.DiscountPrice == 0 ? product.Result.Price : product.Result.DiscountPrice;
                            hasChanged = true;
                        }
                        else if (size == 0)
                        {
                            item.Product = product.Result;
                            //item.Size = _attributeOptionValueRepository.GetById(size).Result;
                            item.Color = _attributeOptionValueRepository.GetById(color).Result;
                            item.Quantity = quantity;
                            item.Price = product.Result.DiscountPrice == 0 ? product.Result.Price : product.Result.DiscountPrice;
                            hasChanged = true;
                        }
                        else if (color == 0)
                        {
                            item.Product = product.Result;
                            item.Size = _attributeOptionValueRepository.GetById(size).Result;
                            //item.Color = _attributeOptionValueRepository.GetById(color).Result;
                            item.Quantity = quantity;
                            item.Price = product.Result.DiscountPrice == 0 ? product.Result.Price : product.Result.DiscountPrice;
                            hasChanged = true;
                        }
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

    }
}