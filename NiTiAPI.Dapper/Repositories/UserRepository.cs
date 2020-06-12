using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Models;
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
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(IConfiguration configuration, IRoleRepository roleRepository,
            UserManager<AppUser> userManager)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        public async Task<UserViewModel> GetById(Guid id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                var result = await conn.QueryAsync<UserViewModel>("Get_User_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();
            }
        }

        public async Task<UserViewModel> GetUserIdRoleId(Guid userid, Guid roleid)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", userid);

                var result = await conn.QueryAsync<UserViewModel>("Get_User_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.Single();
            }           
        }

        public async Task<List<UserViewModel>> GetListUser()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<UserViewModel>("Get_User_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<PagedResult<UserViewModel>> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
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
                    var result = await conn.QueryAsync<UserViewModel>("Get_User_AllKey",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    int totalRow = paramaters.Get<int>("@totalRow");

                    var pagedResult = new PagedResult<UserViewModel>()
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

        public async Task<bool> CreateUser(UserViewModel userVm)
        {            
            var user = new AppUser()
            {
                Id = userVm.Id,
                Avatar = userVm.Avatar,
                FullName = userVm.FullName,
                UserName = userVm.UserName,               
                Email = userVm.Email,     
                PhoneNumber = userVm.PhoneNumber,
                CorporationId = userVm.CorporationId,
                Status = userVm.Status,
                Active = userVm.Active,
                SortOrder = userVm.SortOrder,
                CreateDate = userVm.CreateDate,
                CreateBy = userVm.CreateBy
            };
            try
            {
                var result = await _userManager.CreateAsync(user, userVm.PasswordHash);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<bool> UpdateUser(UserViewModel userVm, string roles)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", userVm.Id);
                paramaters.Add("@Avatar", userVm.Avatar);
                paramaters.Add("@FullName", userVm.FullName);
                paramaters.Add("@Email", userVm.Email);
                paramaters.Add("@PhoneNumber", userVm.PhoneNumber);
                paramaters.Add("@CorporationId", userVm.CorporationId);
                paramaters.Add("@Active", userVm.Active);
                paramaters.Add("@Roles", roles);

                paramaters.Add("@UpdateDate", userVm.UpdateDate);
                paramaters.Add("@UpdateBy", userVm.UpdateBy);
                try
                {
                    await conn.QueryAsync<UserViewModel>(
                        "Update_UserRoles", paramaters, commandType: System.Data.CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> UpdateUserPass(UserViewModel userVm)
        {            
            try
            {
                var user = await _userManager.FindByIdAsync(userVm.Id.ToString());                
  
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, userVm.PasswordHash);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(Guid id, string username)
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
                    await conn.QueryAsync<UserViewModel>(
                       "Delete_User_ById", paramaters, commandType: CommandType.StoredProcedure);
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
