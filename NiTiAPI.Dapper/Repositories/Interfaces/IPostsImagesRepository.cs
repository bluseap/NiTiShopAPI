using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IPostsImagesRepository
    {
        Task<List<PostsImagesViewModel>> GetListPostImages(int postId);

        Task<bool> PostImages(int postId, string images, string userName);

        Task<bool> DeleteImage(long postsImageId, string userName);
    }
}
