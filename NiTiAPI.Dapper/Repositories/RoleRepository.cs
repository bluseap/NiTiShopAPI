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
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<RoleViewModel> GetById(Guid id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                var result = await conn.QueryAsync<RoleViewModel>("Get_Role_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();
            }
        }
       
        public async Task<List<RoleViewModel>> GetListRole()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<RoleViewModel>("Get_Role_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }
       
        public async Task<PagedResult<RoleViewModel>> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
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
                    var result = await conn.QueryAsync<RoleViewModel>("Get_Role_AllKey",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    int totalRow = paramaters.Get<int>("@totalRow");

                    var pagedResult = new PagedResult<RoleViewModel>()
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
       
        public async Task<Boolean> Create(RoleViewModel role)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", role.CorporationId);               
                paramaters.Add("@Name", role.Name);
                paramaters.Add("@Description", role.Description);     
                paramaters.Add("@CreateDate", role.CreateDate);
                paramaters.Add("@CreateBy", role.CreateBy);

                try
                {
                    await conn.QueryAsync<RoleViewModel>(
                        "Create_Role", paramaters, commandType: CommandType.StoredProcedure);  
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       
        public async Task<Boolean> Update(RoleViewModel role)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", role.Id);
                paramaters.Add("@CorporationId", role.CorporationId);              
                paramaters.Add("@Name", role.Name);
                paramaters.Add("@Description", role.Description);               
                paramaters.Add("@UpdateDate", role.UpdateDate);
                paramaters.Add("@UpdateBy", role.UpdateBy);

                try
                {
                    await conn.QueryAsync<RoleViewModel>(
                        "Update_Role", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       
        public async Task<Boolean> Delete(Guid id, string username)
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
                    await conn.QueryAsync<RoleViewModel>(
                       "Delete_Role_ById", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #region Function Permission Action

        public async Task<List<FunctionPermisionViewModel>> GetListFuntionPermissionByRole(Guid roleId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@roleId", roleId);

                var result = await conn.QueryAsync<FunctionPermisionViewModel>("Get_Function_PermisionWithActions_ByRole",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }


        public async Task<List<FunctionPermisionViewModel>> GetListFuntionPermission()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<FunctionPermisionViewModel>("Get_Function_PermisionWithActions",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<Boolean> FunctionPermissionCreateXML(FunctionPermisionViewModel functionpermission)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                
                paramaters.Add("@FunctionPermissionXML", functionpermission.FunctionPermissionXML);
                paramaters.Add("@CreateDate", functionpermission.CreateDate);
                paramaters.Add("@CreateBy", functionpermission.CreateBy);

                try
                {
                    await conn.QueryAsync<FunctionPermisionViewModel>(
                        "Create_FunctionPermissionXML", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion


    }
}
