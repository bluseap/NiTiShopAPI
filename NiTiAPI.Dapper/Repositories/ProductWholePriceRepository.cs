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
    public class ProductWholePriceRepository : IProductWholePriceRepository
    {
        private readonly string _connectionString;

        public ProductWholePriceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<ProductWholePriceViewModel>> GetListProductWholePrice(long productId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@productId ", productId);

                var result = await conn.QueryAsync<ProductWholePriceViewModel>("Get_ProductWholePrice_ByProductId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<bool> CreateProductWholePrice(string productWholePriceXML, string userName, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@productWholePriceXML", productWholePriceXML);
                paramaters.Add("@CreateBy", userName);
                paramaters.Add("@languageId", languageId);

                try
                {
                    await conn.QueryAsync<ProductWholePriceViewModel>(
                        "Create_ProductWholePriceXML", paramaters, commandType: CommandType.StoredProcedure);
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
