using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class AppUserLoginRepository : IAppUserLoginRepository
    {
        private readonly string _connectionString;

        public AppUserLoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }       

        public async Task Create(AppUserLogin appuserlogin)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", appuserlogin.Id);
                paramaters.Add("@UserName", appuserlogin.UserName);
                paramaters.Add("@LoginIpAddress", appuserlogin.LoginIpAddress);
                paramaters.Add("@LoginIp", appuserlogin.LoginIp);
                paramaters.Add("@LoginNameIp", appuserlogin.LoginNameIp);
                paramaters.Add("@LoginIp6Address", appuserlogin.LoginIp6Address);
                paramaters.Add("@LoginLocalIp6Adress", appuserlogin.LoginLocalIp6Adress);
                paramaters.Add("@LoginMacIp", appuserlogin.LoginMacIp);
                paramaters.Add("@StatusContent", appuserlogin.StatusContent);
                paramaters.Add("@CreateDate", appuserlogin.CreateDate);
                paramaters.Add("@CreateBy", appuserlogin.CreateBy);
                await conn.ExecuteAsync("Create_AppUserLogin", paramaters, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Boolean> AppUserLoginAUD(AppUserLoginViewModel appuser, string parameters)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@Id", appuser.Id);
                dynamicParameters.Add("@UserName", appuser.UserName);

                dynamicParameters.Add("@LoginIpAddress", appuser.LoginIpAddress);
                dynamicParameters.Add("@LoginIp", appuser.LoginIp);
                dynamicParameters.Add("@LoginNameIp", appuser.LoginNameIp);
                dynamicParameters.Add("@LoginIp6Address", appuser.LoginIp6Address);
                dynamicParameters.Add("@LoginLocalIp6Adress", appuser.LoginLocalIp6Adress);
                dynamicParameters.Add("@LoginMacIp", appuser.LoginMacIp);

                dynamicParameters.Add("@CreateDate", appuser.CreateDate);
                dynamicParameters.Add("@CreateBy", appuser.CreateBy);

                dynamicParameters.Add("@parameters", parameters);
                try
                {
                    var query = await sqlConnection.QueryAsync<AppUserLoginViewModel>(
                        "AppUserLoginAUD", dynamicParameters, commandType: CommandType.StoredProcedure);

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
