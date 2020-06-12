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
    public class PostController : BaseController
    {
        private readonly IPostRepository _post;
        private readonly IPostsImagesRepository _postsImages;

        public PostController(IPostRepository post, IPostsImagesRepository postsImages)
        {
            _post = post;
            _postsImages = postsImages;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _post.GetById(id, "vi-VN");
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetImagePostId(int postId)
        {
            var model = await _postsImages.GetListPostImages(postId);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(string keyword, string culture, int corporationId, int categoryNewsId,
            int pageIndex, int pageSize)
        {
            var model = await _post.GetPaging(keyword, culture, corporationId , categoryNewsId, pageIndex, pageSize);
            return new OkObjectResult(model);
        }       

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_POST, ActionCode.CREATE)]
        public async Task<IActionResult> SavePostsImages(int postId, string images, string username)
        {
            var productImages = await _postsImages.PostImages(postId, images, username);
            return new OkObjectResult(productImages);
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_POST, ActionCode.CREATE)]
        public async Task<IActionResult> CreatePost(PostViewModel postVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                postVm.CreatedDate = DateTime.Now;
                postVm.PublishedDate  = DateTime.Now;
                postVm.HotDate = DateTime.Now;
                postVm.NewDate = DateTime.Now;
                postVm.CreateDate = DateTime.Now;
                var post = await _post.Create(postVm);

                return new OkObjectResult(post);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_POST, ActionCode.CREATE)]
        public async Task<IActionResult> CreatePostImageXML(PostViewModel postVm, string listImageXML)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                postVm.CreatedDate = DateTime.Now;
                postVm.PublishedDate = DateTime.Now;
                postVm.HotDate = DateTime.Now;
                postVm.NewDate = DateTime.Now;

                postVm.ListImageXML = listImageXML;

                postVm.CreateDate = DateTime.Now;
                var post = await _post.CreatePostImageXML(postVm, listImageXML);

                return new OkObjectResult(post);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_POST, ActionCode.UPDATE)]
        public async Task<IActionResult> UpdatePost(PostViewModel postVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                postVm.CreatedDate = DateTime.Now;
                postVm.PublishedDate = DateTime.Now;
                postVm.HotDate = DateTime.Now;
                postVm.NewDate = DateTime.Now;
                
                postVm.UpdateDate = DateTime.Now;

                var post = await _post.Update(postVm);
                return new OkObjectResult(post);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_POST, ActionCode.DELETE)]
        public async Task<IActionResult> DeletePost(int Id, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var post = await _post.Delete(Id, username);
                return new OkObjectResult(post);
            }
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.NEWS_POST, ActionCode.DELETE)]
        public async Task<IActionResult> DeletePostImage(long Id, string username)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                var post = await _postsImages.DeleteImage(Id, username);
                return new OkObjectResult(post);
            }
        }


    }
}