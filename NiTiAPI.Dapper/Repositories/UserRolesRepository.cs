using Dapper;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class UserRolesRepository: IUserRolesRepository
    {
        private readonly string _connectionString;
       

        public UserRolesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
          
        }

        public async Task<List<UserRolesViewModel>> GetListUser(Guid userid)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@userid", userid);

                var result = await conn.QueryAsync<UserRolesViewModel>("Get_UserRoles_ByUser",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<UserRolesViewModel>> GetListRole(Guid roleid)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@roleid", roleid);

                var result = await conn.QueryAsync<UserRolesViewModel>("Get_UserRoles_ByRole",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<UserRolesViewModel> GetUserRole(Guid userid, Guid roleid)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@userid", userid);
                paramaters.Add("@roleid", roleid);

                var result = await conn.QueryAsync<UserRolesViewModel>("Get_UserRoles_ByUserRole",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();
            }
        }

        public async Task<ListTResult<UserRolesViewModel>> GetListUserRole(Guid userId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", userId);

                try
                {
                    var userview = await conn.QueryAsync<UserViewModel>("Get_User_ById",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    var user = userview.Single();
                    var roles = await GetListUser(userId);

                    var userroles = new ListTResult<UserRolesViewModel>()
                    {
                        Items = roles.ToList(),
                        Id = user.Id,
                        FullName = user.FullName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        CorporationId = user.CorporationId,
                        Status = user.Status,
                        Avatar = user.Avatar
                    };

                    return userroles;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }




    }
}
