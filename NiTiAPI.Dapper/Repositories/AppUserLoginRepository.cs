using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

    }
}
