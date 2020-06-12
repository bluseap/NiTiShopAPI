using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IProductImagesRepository
    {
        Task<List<ProductImagesViewModel>> GetListProductImages(long productId);

        Task<bool> ProductImages(long productId, string images, string userName);

        Task<bool> DeleteImage(long productImageId, string userName);
    }
}
