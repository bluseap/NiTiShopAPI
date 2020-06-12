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
    public class ProductQuantitiesRepository : IProductQuantitiesRepository
    {
        private readonly string _connectionString;

        public ProductQuantitiesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<ProductQuantitiesViewModel>> GetListProductQuantities(long productId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@productId ", productId);

                var result = await conn.QueryAsync<ProductQuantitiesViewModel>("Get_ProductQuantities_ByProductId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<bool> CreateProductQuantities(string productQuantiesXML, string userName, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@productQuantiesXML", productQuantiesXML);
                paramaters.Add("@CreateBy", userName);
                paramaters.Add("@languageId", languageId);

                try
                {
                    await conn.QueryAsync<ProductQuantitiesViewModel>(
                        "Create_ProductQuantitiesXML", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> DeleteQuantities(long productQuantitiesId, string userName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", productQuantitiesId);
                paramaters.Add("@CreateBy", userName);

                try
                {
                    await conn.QueryAsync<ProductQuantitiesViewModel>(
                        "Delete_ProductQuantities_ById", paramaters, commandType: CommandType.StoredProcedure);
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
