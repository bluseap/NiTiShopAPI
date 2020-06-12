using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;

namespace NiTiAPI.Dapper.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;       

        public ProductRepository(IConfiguration configuration)
        {         
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPagingProductCate(string corporationName, string catelogyId, 
            string culture, string keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationName", corporationName);                
                paramaters.Add("@categoryId", catelogyId);
                paramaters.Add("@languageId", culture);
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);               

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<ProductViewModel>("Get_Product_PagingProductCate", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<ProductViewModel>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPagingProductCateCorId(int corporationId, string catelogyId,
            string culture, string keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@categoryId", catelogyId);
                paramaters.Add("@languageId", culture);
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<ProductViewModel>("Get_Product_PagingProductCateCorId", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<ProductViewModel>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task<List<ProductViewModel>> GetListProductCatelogCorName(string corporationName, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@corporationName", corporationName);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<ProductViewModel>("Get_ProductCatalog_ByCorName", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<ProductViewModel>> GetListProductCatelogCorId1(int corporationId, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<ProductViewModel>("Get_ProductCatalog_ByCorTopId", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<ProductViewModel>> GetListProductCorporationName(string corporationName, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
               
                paramaters.Add("@corporationName", corporationName);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<ProductViewModel>("Get_Product_ByCorpoName", paramaters, null, null, System.Data.CommandType.StoredProcedure);
               
                return result.ToList();
            }
        }

        public async Task<List<ProductViewModel>> GetListProductCorNameTop(string corporationName, string language, int top)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@corporationName", corporationName);
                paramaters.Add("@language", language);
                paramaters.Add("@top", top);

                var result = await conn.QueryAsync<ProductViewModel>("Get_ProductCatalog_ByCorNameTop", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<ProductViewModel>> GetListProductCorTopId2(int corporationId, string language, int top)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@language", language);
                paramaters.Add("@top", top);

                var result = await conn.QueryAsync<ProductViewModel>("Get_ProductCatalog_ByCorTopId2", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<ProductViewModel> GetById(long id, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@language", culture);

                var result = await conn.QueryAsync<ProductViewModel>("Get_Product_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<PagedResult<ProductViewModel>> GetPagingProduct(string keyword, int corporationId, int categoryId,
            int pageIndex, int pageSize, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@categoryId", categoryId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@languageId", culture);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<ProductViewModel>("Get_Product_AllKey", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<ProductViewModel>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task<bool> CreateProduct(ProductViewModel product, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", product.CorporationId);
                paramaters.Add("@CategoryId", product.CategoryId);
                paramaters.Add("@name", product.Name);
                paramaters.Add("@description", product.Description);
                paramaters.Add("@AttributeValueText", product.AttributeValueText);
                paramaters.Add("@Price", product.Price);
                paramaters.Add("@OriginalPrice", product.OriginalPrice);
                paramaters.Add("@DiscountPrice", product.DiscountPrice);
                paramaters.Add("@ImageUrl", product.ImageUrl);
                paramaters.Add("@contents", product.Contents);

                paramaters.Add("@SeoTitle", product.SeoTitle);
                paramaters.Add("@SeoAlias", product.SeoAlias);
                paramaters.Add("@SeoKeyword", product.SeoKeyword);
                paramaters.Add("@SeoDescription", product.SeoDescription);
                paramaters.Add("@SeoTags", product.SeoTags);             
               
                paramaters.Add("@IsActive", product.IsActive);
                paramaters.Add("@HomeFlag", product.HomeFlag);
                paramaters.Add("@HotFlag", product.HotFlag);

                paramaters.Add("@language", culture);

                paramaters.Add("@CreateDate", product.CreateDate);
                paramaters.Add("@CreateBy", product.CreateBy);
                try
                {
                    await conn.QueryAsync<ProductViewModel>(
                        "Create_Product", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> UpdateProduct(ProductViewModel product, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@id", product.Id);

                paramaters.Add("@CorporationId", product.CorporationId);
                paramaters.Add("@CategoryId", product.CategoryId);
                paramaters.Add("@name", product.Name);
                paramaters.Add("@description", product.Description);
                paramaters.Add("@AttributeValueText", product.AttributeValueText);
                paramaters.Add("@Price", product.Price);
                paramaters.Add("@OriginalPrice", product.OriginalPrice);
                paramaters.Add("@DiscountPrice", product.DiscountPrice);
                paramaters.Add("@ImageUrl", product.ImageUrl);
                paramaters.Add("@contents", product.Contents);

                paramaters.Add("@SeoTitle", product.SeoTitle);
                paramaters.Add("@SeoAlias", product.SeoAlias);
                paramaters.Add("@SeoKeyword", product.SeoKeyword);
                paramaters.Add("@SeoDescription", product.SeoDescription);
                paramaters.Add("@SeoTags", product.SeoTags);

                paramaters.Add("@IsActive", product.IsActive);
                paramaters.Add("@HomeFlag", product.HomeFlag);
                paramaters.Add("@HotFlag", product.HotFlag);

                paramaters.Add("@language", culture);

                paramaters.Add("@UpdateDate", product.UpdateDate);
                paramaters.Add("@UpdateBy", product.UpdateBy);
                try
                {
                    await conn.QueryAsync<ProductViewModel>(
                        "Update_Product", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> DeleteProduct(long id, string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@id", id);
                paramaters.Add("@UserName", username);

                try
                {
                    await conn.QueryAsync<ProductViewModel>(
                       "Delete_Product_ById", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }       


        #region Product
        public async Task<IEnumerable<Product>> GetAllAsync(string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@language", culture);

                var result = await conn.QueryAsync<Product>("Get_Product_All", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<Product> GetByIdAsync(int id, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@language", culture);

                var result = await conn.QueryAsync<Product>("Get_Product_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();
            }
        }

        public async Task<PagedResult<Product>> GetPaging(string keyword, string culture, int categoryId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@categoryId", categoryId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@language", culture);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<Product>("Get_Product_AllPaging", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<Product>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task<int> Create(string culture, Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@name", product.Name);
                paramaters.Add("@description", product.Description);
                paramaters.Add("@content", product.Content);
                paramaters.Add("@seoDescription", product.SeoDescription);
                paramaters.Add("@seoAlias", product.SeoAlias);
                paramaters.Add("@seoTitle", product.SeoTitle);
                paramaters.Add("@seoKeyword", product.SeoKeyword);
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActive", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);
                paramaters.Add("@language", culture);
                paramaters.Add("@categoryIds", product.CategoryIds);
                paramaters.Add("@id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var result = await conn.ExecuteAsync("Create_Product", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int newId = paramaters.Get<int>("@id");
                return newId;
            }
        }

        public async Task Update(string culture, int id, Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@name", product.Name);
                paramaters.Add("@description", product.Description);
                paramaters.Add("@content", product.Content);
                paramaters.Add("@seoDescription", product.SeoDescription);
                paramaters.Add("@seoAlias", product.SeoAlias);
                paramaters.Add("@seoTitle", product.SeoTitle);
                paramaters.Add("@seoKeyword", product.SeoKeyword);
                paramaters.Add("@sku", product.Sku);
                paramaters.Add("@price", product.Price);
                paramaters.Add("@isActive", product.IsActive);
                paramaters.Add("@imageUrl", product.ImageUrl);
                paramaters.Add("@language", culture);
                paramaters.Add("@categoryIds", product.CategoryIds);
                await conn.ExecuteAsync("Update_Product", paramaters, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                await conn.ExecuteAsync("Delete_Product_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<List<ProductAttributeViewModel>> GetAttributes(int id, string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@language", culture);

                var result = await conn.QueryAsync<ProductAttributeViewModel>("Get_Product_Attributes", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<PagedResult<Product>> SearchByAttributes(string keyword, string culture,
            int categoryId, string size, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@categoryId", categoryId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@language", culture);
                paramaters.Add("@size",size);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32,
                    direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<Product>("[Search_Product_ByAttributes]",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<Product>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }
        #endregion Propduct

    }
}
