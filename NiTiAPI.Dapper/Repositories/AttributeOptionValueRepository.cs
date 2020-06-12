using Dapper;
using Microsoft.Extensions.Configuration;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories
{
    public class AttributeOptionValueRepository : IAttributeOptionValueRepository
    {
        private readonly string _connectionString;

        public AttributeOptionValueRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }       

        public async Task<List<AttributeOptionValueViewModel>> GetListByAttribute(int attributeId, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@attributeId", attributeId);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<AttributeOptionValueViewModel>("Get_AttributeOptionValue_ByAttributeId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<AttributeOptionValueViewModel>> GetListByAttributeSize(long productId, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@productId", productId);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<AttributeOptionValueViewModel>("Get_AttributeOptionValue_SizeByProductId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<AttributeOptionValueViewModel> GetByProductId(long productId, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@productId", productId);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<AttributeOptionValueViewModel>("Get_AttributeOptionValue_ValueByProductId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<List<AttributeOptionValueViewModel>> GetByAttriCodeSize(string codeSize, string language)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@codeSize", codeSize);
                paramaters.Add("@language", language);

                var result = await conn.QueryAsync<AttributeOptionValueViewModel>("Get_AttributeOptionValue_ByAttriCodeSize",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<AttributeOptionValueViewModel> GetById(int attributeOptionValueId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@attributeOptionValueId", attributeOptionValueId);               

                var result = await conn.QueryAsync<AttributeOptionValueViewModel>("Get_AttributeOptionValue_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<List<AttributeOptionValueViewModel>> GetListAll()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<AttributeOptionValueViewModel>("Get_AttributeOptionValue_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

    }
}
