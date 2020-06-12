using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<ProductViewModel>> GetAllPagingProductCate(string corporationName, string catelogyId, string culture,
            string keyword, int pageIndex, int pageSize);

        Task<List<ProductViewModel>> GetListProductCatelogCorName(string corporationName, string language);

        Task<PagedResult<ProductViewModel>> GetAllPagingProductCateCorId(int corporationId, string catelogyId,
            string culture, string keyword, int pageIndex, int pageSize);

        Task<List<ProductViewModel>> GetListProductCatelogCorId1(int corporationId, string language);

        Task<List<ProductViewModel>> GetListProductCorporationName(string corporationName, string language);

        Task<List<ProductViewModel>> GetListProductCorNameTop(string corporationName, string language, int top);

        Task<List<ProductViewModel>> GetListProductCorTopId2(int corporationId, string language, int top);

        Task<ProductViewModel> GetById(long id, string culture);

        Task<PagedResult<ProductViewModel>> GetPagingProduct(string keyword, int corporationId, int categoryId,
            int pageIndex, int pageSize, string culture);

        Task<bool> CreateProduct(ProductViewModel product, string culture);

        Task<bool> UpdateProduct(ProductViewModel product, string culture);

        Task<bool> DeleteProduct(long id, string username);     


        #region Product

        Task<IEnumerable<Product>> GetAllAsync(string culture);

        Task<Product> GetByIdAsync(int id, string culture);

        Task<PagedResult<Product>> GetPaging(string keyword, string culture, int categoryId, int pageIndex, int pageSize);

        Task<int> Create(string culture, Product product);

        Task Update(string culture, int id, Product product);

        Task Delete(int id);

        Task<List<ProductAttributeViewModel>> GetAttributes(int id, string culture);

        Task<PagedResult<Product>> SearchByAttributes(string keyword, string culture,
            int categoryId,string size, int pageIndex, int pageSize);
        
        #endregion Product

    }
}
