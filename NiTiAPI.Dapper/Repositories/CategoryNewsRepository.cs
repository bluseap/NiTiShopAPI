using Dapper;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class CategoryNewsRepository :  ICategoryNewsRepository
    {
        private readonly string _connectionString;

        public CategoryNewsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<CategoryNewsViewModel> GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                var result = await conn.QueryAsync<CategoryNewsViewModel>("Get_CategoryNews_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<List<CategoryNewsViewModel>> GetListCategory()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<CategoryNewsViewModel>("Get_CategoryNews_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<CategoryNewsViewModel>> GetListCateByCor(string corporationName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationName", corporationName);

                var result = await conn.QueryAsync<CategoryNewsViewModel>("Get_CategoryNews_ByCorpo",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public List<CategoryNewsViewModel> GetListCateByCorId(int corporationId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationId", corporationId);

                var result = conn.QueryAsync<CategoryNewsViewModel>("Get_CategoryNews_ByCorpoId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.Result.ToList();
            }
        }

        public List<CategoryNewsViewModel> GetListHomeCateByCorLangId(int corporationId, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@languageId", languageId);

                var result = conn.QueryAsync<CategoryNewsViewModel>("Get_CategoryNews_HomeByCorpoLanguageId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.Result.ToList();
            }
        }

        public async Task<PagedResult<CategoryNewsViewModel>> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@corporationId", cororationId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                try
                {
                    var result = await conn.QueryAsync<CategoryNewsViewModel>("Get_CategoryNew_AllKey",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    int totalRow = paramaters.Get<int>("@totalRow");

                    var pagedResult = new PagedResult<CategoryNewsViewModel>()
                    {
                        Items = result.ToList(),
                        TotalRow = totalRow,
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    };
                    return pagedResult;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Create(CategoryNewsViewModel category)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", category.CorporationId);
                paramaters.Add("@Name", category.Name);
                paramaters.Add("@Description", category.Description);
                paramaters.Add("@SeoAlias", category.SeoAlias);
                paramaters.Add("@SeoTitle", category.SeoTitle);
                paramaters.Add("@SeoKeyword", category.SeoKeyword);
                paramaters.Add("@SeoDescription", category.SeoDescription);
                paramaters.Add("@ParentId", category.ParentId);
                paramaters.Add("@SortOrder", category.SortOrder);

                paramaters.Add("@ShowInMenu", category.ShowInMenu);
                paramaters.Add("@ShowInHome", category.ShowInHome);
                paramaters.Add("@Thumbnail", category.Thumbnail);

                paramaters.Add("@IsActive", category.IsActive);

                paramaters.Add("@CreateDate", category.CreateDate);
                paramaters.Add("@CreateBy", category.CreateBy);
                try
                {
                    await conn.QueryAsync<CategoryNewsViewModel>(
                        "Create_CategoryNews", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Update(CategoryNewsViewModel category)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", category.Id);
                paramaters.Add("@CorporationId", category.CorporationId);
                paramaters.Add("@Name", category.Name);
                paramaters.Add("@Description", category.Description);
                paramaters.Add("@SeoAlias", category.SeoAlias);
                paramaters.Add("@SeoTitle", category.SeoTitle);
                paramaters.Add("@SeoKeyword", category.SeoKeyword);
                paramaters.Add("@SeoDescription", category.SeoDescription);
                paramaters.Add("@ParentId", category.ParentId);
                paramaters.Add("@SortOrder", category.SortOrder);

                paramaters.Add("@ShowInMenu", category.ShowInMenu);
                paramaters.Add("@ShowInHome", category.ShowInHome);
                paramaters.Add("@Thumbnail", category.Thumbnail);

                paramaters.Add("@IsActive", category.IsActive);

                paramaters.Add("@UpdateDate", category.UpdateDate);
                paramaters.Add("@UpdateBy", category.UpdateBy);
                try
                {
                    await conn.QueryAsync<CategoryNewsViewModel>(
                        "Update_CategoryNews", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> UpdateParent(int fromParent, int toParent, int parameter, string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@FromParentId", fromParent);
                paramaters.Add("@ToParentId", toParent);
                paramaters.Add("@parameter", parameter);
                paramaters.Add("@UpdateBy", username);

                try
                {
                    await conn.QueryAsync<CategoryNewsViewModel>(
                        "Update_CategoryNews_ParentIdFromTo", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Delete(int id, string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", id);
                paramaters.Add("@UserName", username);

                try
                {
                    await conn.QueryAsync<CategoryNewsViewModel>(
                       "Delete_CategoryNews_ById", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
