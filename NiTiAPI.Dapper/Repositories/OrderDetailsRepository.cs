using Dapper;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly string _connectionString;

        public OrderDetailsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<OrderDetailsViewModel>> GetListOrderByCreateDate(DateTime fromDate, DateTime toDate, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@fromDate ", fromDate);
                paramaters.Add("@toDate ", toDate); 
                paramaters.Add("@languageId ", languageId);

                var result = await conn.QueryAsync<OrderDetailsViewModel>("Get_OrderDetails_ByCreateDate",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<OrderDetailsViewModel>> GetListOrderDetails(long orderId, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@orderId ", orderId);
                paramaters.Add("@languageId ", languageId);

                var result = await conn.QueryAsync<OrderDetailsViewModel>("Get_OrderDetails_ByOrderId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<bool> CreateOrderDetailsXML(string lisetOrderXML, string userName, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@lisetOrderXML", lisetOrderXML);
                paramaters.Add("@CreateBy", userName);
                paramaters.Add("@languageId", languageId);

                try
                {
                    await conn.QueryAsync<OrderDetailsViewModel>(
                        "Create_OrderDetailsXML", paramaters, commandType: CommandType.StoredProcedure);
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
