using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class ContactRepository: IContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<ContactViewModel> GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);

                var result = await conn.QueryAsync<ContactViewModel>("Get_Contact_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<ContactViewModel> GetByCorName(string corporationName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@CorporatiomName", corporationName);

                var result = await conn.QueryAsync<ContactViewModel>("Get_Contact_ByCorporationName",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<ContactViewModel> GetByCorId(int corporationId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@CorporationId", corporationId);

                var result = await conn.QueryAsync<ContactViewModel>("Get_Contact_ByCorporationId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<List<ContactViewModel>> GetListContact()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                var result = await conn.QueryAsync<ContactViewModel>("Get_Contact_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }      

        public async Task<PagedResult<ContactViewModel>> GetPaging(string keyword, int cororationId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@corporationId", cororationId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                try
                {
                    var result = await conn.QueryAsync<ContactViewModel>("Get_Contact_AllKey",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    int totalRow = paramaters.Get<int>("@totalRow");

                    var pagedResult = new PagedResult<ContactViewModel>()
                    {
                        Items = result.ToList(),
                        TotalRow = totalRow,
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    };
                    return pagedResult;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Create(ContactViewModel contact)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", contact.CorporationId);
                paramaters.Add("@Address", contact.Address);
                paramaters.Add("@Email", contact.Email);
                paramaters.Add("@Lat", contact.Lat);
                paramaters.Add("@Lng", contact.Lng);
                paramaters.Add("@Name", contact.Name);
                paramaters.Add("@Other", contact.Other);
                paramaters.Add("@Phone", contact.Phone);
                paramaters.Add("@Website", contact.Website);
                paramaters.Add("@SortOrder", contact.SortOrder);            

                paramaters.Add("@CreateDate", contact.CreateDate);
                paramaters.Add("@CreateBy", contact.CreateBy);
                try
                {
                    await conn.QueryAsync<ContactViewModel>(
                        "Create_Contact", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Update(ContactViewModel contact)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", contact.Id);
                paramaters.Add("@CorporationId", contact.CorporationId);
                paramaters.Add("@Address", contact.Address);
                paramaters.Add("@Email", contact.Email);
                paramaters.Add("@Lat", contact.Lat);
                paramaters.Add("@Lng", contact.Lng);
                paramaters.Add("@Name", contact.Name);
                paramaters.Add("@Other", contact.Other);
                paramaters.Add("@Phone", contact.Phone);
                paramaters.Add("@Website", contact.Website);
                paramaters.Add("@SortOrder", contact.SortOrder);

                paramaters.Add("@UpdateDate", contact.UpdateDate);
                paramaters.Add("@UpdateBy", contact.UpdateBy);
                try
                {
                    await conn.QueryAsync<ContactViewModel>(
                        "Update_Contact", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }       

        public async Task<bool> Delete(int id, string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", id);
                paramaters.Add("@UserName", username);

                try
                {
                    await conn.QueryAsync<ContactViewModel>(
                       "Delete_Contact_ById", paramaters, commandType: CommandType.StoredProcedure);
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
