using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class CorporationRepository : ICorporationRepository
    {
        private readonly string _connectionString;      

        public CorporationRepository(IConfiguration configuration)
        {       
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<CorporationViewModel> GetByOrderId(long orderId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@orderId", orderId);

                var result = await conn.QueryAsync<CorporationViewModel>("Get_Corporation_ByOrderId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();
            }
        }

        public async Task<CorporationViewModel> GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                var result = await conn.QueryAsync<CorporationViewModel>("Get_Corporation_ById", 
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.Single();
            }
        }

        public async Task<List<CorporationViewModel>> GetListCorporations()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();             

                var result = await conn.QueryAsync<CorporationViewModel>("Get_Corporation_All", 
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }
     
        public async Task<PagedResult<CorporationViewModel>> GetPaging(string keyword, int corporationId, int serviceId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@serviceId", serviceId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);              

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<CorporationViewModel>("Get_Corporation_AllPaging", 
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<CorporationViewModel>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }
      
        public async Task<Boolean> Create(CorporationViewModel corporation)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                
                paramaters.Add("@ServiceId", corporation.ServiceId);
                paramaters.Add("@Name", corporation.Name);
                paramaters.Add("@Aliases", corporation.Aliases);
                paramaters.Add("@Address", corporation.Address);
                paramaters.Add("@Email", corporation.Email);
                paramaters.Add("@Image", corporation.Image);
                paramaters.Add("@ImageLogo", corporation.ImageLogo);             
                paramaters.Add("@MobileNumber", corporation.MobileNumber);
                paramaters.Add("@PhoneNumber", corporation.PhoneNumber);
                paramaters.Add("@TaxNumber", corporation.TaxNumber);
                paramaters.Add("@WebName", corporation.WebName);
                paramaters.Add("@ParentId", corporation.ParentId);
                paramaters.Add("@Description", corporation.Description);
                paramaters.Add("@Status", corporation.Status);
                paramaters.Add("@SortOrder", corporation.SortOrder);
                paramaters.Add("@CreateDate", corporation.CreateDate);
                paramaters.Add("@CreateBy", corporation.CreateBy);

                try
                {
                    var query = await conn.QueryAsync<CorporationViewModel>(
                        "Create_Corporation", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
      
        public async Task<Boolean> Update(int id, CorporationViewModel corporation)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", id);
                paramaters.Add("@ServiceId", corporation.ServiceId);
                paramaters.Add("@Name", corporation.Name);
                paramaters.Add("@Aliases", corporation.Aliases);
                paramaters.Add("@Address", corporation.Address);
                paramaters.Add("@Email", corporation.Email);
                paramaters.Add("@Image", corporation.Image);
                paramaters.Add("@ImageLogo", corporation.ImageLogo);
                paramaters.Add("@MobileNumber", corporation.MobileNumber);
                paramaters.Add("@PhoneNumber", corporation.PhoneNumber);
                paramaters.Add("@TaxNumber", corporation.TaxNumber);
                paramaters.Add("@WebName", corporation.WebName);
                paramaters.Add("@ParentId", corporation.ParentId);
                paramaters.Add("@Description", corporation.Description);
                paramaters.Add("@Status", corporation.Status);
                paramaters.Add("@SortOrder", corporation.SortOrder);
                paramaters.Add("@UpdateDate", corporation.UpdateDate);
                paramaters.Add("@UpdateBy", corporation.UpdateBy);

                try
                {
                    var query = await conn.QueryAsync<CorporationViewModel>(
                        "Update_Corporation", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       
        public async Task<Boolean> Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", id);

                try
                {
                    var query = await conn.QueryAsync<CorporationViewModel>(
                       "Delete_Corporation", paramaters, commandType: CommandType.StoredProcedure);
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
