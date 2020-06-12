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
    public class CategoriesRepository: ICategoriesRepository
    {
        private readonly string _connectionString;

        public CategoriesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<CategoriesViewModel> GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                var result = await conn.QueryAsync<CategoriesViewModel>("Get_Category_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<List<CategoriesViewModel>> GetListCategory()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<CategoriesViewModel>("Get_Category_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<CategoriesViewModel>> GetListCateByCor(string corporationName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationName", corporationName);

                var result = await conn.QueryAsync<CategoriesViewModel>("Get_Category_ByCorpo",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<CategoriesViewModel>> GetListCateByCorId(int corporationId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@corporationId", corporationId);

                var result = await conn.QueryAsync<CategoriesViewModel>("Get_Category_ByCorpoId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<PagedResult<CategoriesViewModel>> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
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
                    var result = await conn.QueryAsync<CategoriesViewModel>("Get_Category_AllKey",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    int totalRow = paramaters.Get<int>("@totalRow");

                    var pagedResult = new PagedResult<CategoriesViewModel>()
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

        public async Task<bool> Create(CategoriesViewModel category)
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
                paramaters.Add("@IsActive", category.IsActive);

                paramaters.Add("@CreateDate", category.CreateDate);
                paramaters.Add("@CreateBy", category.CreateBy);
                try
                {
                    await conn.QueryAsync<CategoriesViewModel>(
                        "Create_Category", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Update(CategoriesViewModel category)
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
                paramaters.Add("@IsActive", category.IsActive);

                paramaters.Add("@UpdateDate", category.UpdateDate);
                paramaters.Add("@UpdateBy", category.UpdateBy);
                try
                {
                    await conn.QueryAsync<CategoriesViewModel>(
                        "Update_Category", paramaters, commandType: CommandType.StoredProcedure);
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
                    await conn.QueryAsync<CategoriesViewModel>(
                        "Update_Category_ParentIdFromTo", paramaters, commandType: CommandType.StoredProcedure);
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
                    await conn.QueryAsync<CategoriesViewModel>(
                       "Delete_Category_ById", paramaters, commandType: CommandType.StoredProcedure);
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
