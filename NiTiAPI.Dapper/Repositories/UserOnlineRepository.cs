using Dapper;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class UserOnlineRepository :  IUserOnlineRepository
    {
        private readonly string _connectionString;

        public UserOnlineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<UserOnlineViewModel>> GetListCorporation(int Id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", Id);

                var result = await conn.QueryAsync<UserOnlineViewModel>("Get_UseOnline_ByCorId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<bool> AddUserOnline(int corporationId, int CountUser, string notes)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", corporationId);
                paramaters.Add("@CountUser", CountUser);
                paramaters.Add("@Notes", notes);                

                try
                {
                    await conn.QueryAsync<UserOnlineViewModel>(
                        "Create_UserOnline", paramaters, commandType: CommandType.StoredProcedure);
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
