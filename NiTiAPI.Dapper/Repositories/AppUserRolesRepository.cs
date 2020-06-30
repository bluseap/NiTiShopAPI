using Dapper;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class AppUserRolesRepository : IAppUserRolesRepository
    {
        private readonly string _connectionString;

        public AppUserRolesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<AppUserRolesViewModel>> GetListpUserRoles(int coporationId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@CoporationId", coporationId);

                var result = await conn.QueryAsync<AppUserRolesViewModel>("Get_AppUserRoles_ByCorporationId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

    }
}
