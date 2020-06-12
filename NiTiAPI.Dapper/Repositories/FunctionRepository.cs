using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class FunctionRepository : IFunctionRepository
    {
        private readonly string _connectionString;

        public FunctionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<FunctionViewModel> GetById(string id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                try
                {
                    var result = await conn.QueryAsync<FunctionViewModel>("Get_Function_ById", paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    return result.SingleOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }               
            }
        }       

        public async Task<List<FunctionViewModel>> GetAll(string filter)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@filter", filter);

                try
                {
                    var query = await conn.QueryAsync<FunctionViewModel>("Get_Function_filter", paramaters,
                        commandType: CommandType.StoredProcedure);

                    return query.AsList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }               
            }
        }
       


    }
}
