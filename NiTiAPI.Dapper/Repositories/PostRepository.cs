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
    public class PostRepository : IPostRepository
    {
        private readonly string _connectionString;

        public PostRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<PostViewModel>> GetAll(string culture)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@language", culture);

                var result = await conn.QueryAsync<PostViewModel>("Get_Posts_All",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<PostViewModel> GetById(int id, string languageId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@id", id);
                paramaters.Add("@language", languageId);

                var result = await conn.QueryAsync<PostViewModel>("Get_Posts_ById",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        public async Task<PagedResult<PostViewModel>> GetPaging(string keyword, string culture, int corporationId, int categoryNewsId, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@corporationId", corporationId);
                paramaters.Add("@categoryNewsId", categoryNewsId);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@language", culture);

                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                try
                {
                    var result = await conn.QueryAsync<PostViewModel>("Get_Posts_AllPaging",
                        paramaters, null, null, System.Data.CommandType.StoredProcedure);

                    int totalRow = paramaters.Get<int>("@totalRow");

                    var pagedResult = new PagedResult<PostViewModel>()
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

        public async Task<bool> Create(PostViewModel posts)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", posts.CorporationId);
                paramaters.Add("@CategoryNewsId", posts.CategoryNewsId);
                paramaters.Add("@Thumbnail", posts.Thumbnail);
                paramaters.Add("@Image", posts.Image);
                paramaters.Add("@ImageCaption", posts.ImageCaption);
                paramaters.Add("@CreatedDate", posts.CreatedDate);
                paramaters.Add("@PublishedDate", posts.PublishedDate);
                paramaters.Add("@Source", posts.Source);               
                paramaters.Add("@Status", posts.Status);
                paramaters.Add("@HotDate", posts.HotDate);
                paramaters.Add("@NewDate", posts.NewDate);
                paramaters.Add("@IsActive", posts.IsActive);
                paramaters.Add("@Title", posts.Title);
                paramaters.Add("@contents", posts.Content);
                paramaters.Add("@Description", posts.Description);
                paramaters.Add("@SeoAlias", posts.SeoAlias);
                paramaters.Add("@SeoTitle", posts.SeoTitle);
                paramaters.Add("@SeoMetaKeywords", posts.SeoMetaKeywords);
                paramaters.Add("@SeoMetaDescription", posts.SeoMetaDescription);
                paramaters.Add("@SeoTags", posts.SeoTags);
                paramaters.Add("@LanguageId", posts.LanguageId);
                paramaters.Add("@CreateDate", posts.CreateDate);
                paramaters.Add("@CreateBy", posts.CreateBy);

                try
                {
                    await conn.QueryAsync<PostViewModel>(
                        "Create_Posts", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> CreatePostImageXML(PostViewModel posts, string listImageXML)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@CorporationId", posts.CorporationId);
                paramaters.Add("@CategoryNewsId", posts.CategoryNewsId);
                paramaters.Add("@Thumbnail", posts.Thumbnail);
                paramaters.Add("@Image", posts.Image);
                paramaters.Add("@ImageCaption", posts.ImageCaption);
                paramaters.Add("@CreatedDate", posts.CreatedDate);
                paramaters.Add("@PublishedDate", posts.PublishedDate);
                paramaters.Add("@Source", posts.Source);
                paramaters.Add("@Status", posts.Status);
                paramaters.Add("@HotDate", posts.HotDate);
                paramaters.Add("@NewDate", posts.NewDate);
                paramaters.Add("@IsActive", posts.IsActive);
                paramaters.Add("@Title", posts.Title);
                paramaters.Add("@contents", posts.Content);
                paramaters.Add("@Description", posts.Description);
                paramaters.Add("@SeoAlias", posts.SeoAlias);
                paramaters.Add("@SeoTitle", posts.SeoTitle);
                paramaters.Add("@SeoMetaKeywords", posts.SeoMetaKeywords);
                paramaters.Add("@SeoMetaDescription", posts.SeoMetaDescription);
                paramaters.Add("@SeoTags", posts.SeoTags);
                paramaters.Add("@LanguageId", posts.LanguageId);

                paramaters.Add("@listImagesXML", listImageXML);

                paramaters.Add("@CreateDate", posts.CreateDate);
                paramaters.Add("@CreateBy", posts.CreateBy);

                try
                {
                    await conn.QueryAsync<PostViewModel>(
                        "Create_Posts_ImagesXML", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> Update(PostViewModel posts)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();

                paramaters.Add("@id", posts.Id);
                paramaters.Add("@CorporationId", posts.CorporationId);
                paramaters.Add("@CategoryNewsId", posts.CategoryNewsId);
                paramaters.Add("@Thumbnail", posts.Thumbnail);
                paramaters.Add("@Image", posts.Image);
                paramaters.Add("@ImageCaption", posts.ImageCaption);
                paramaters.Add("@CreatedDate", posts.CreatedDate);
                paramaters.Add("@PublishedDate", posts.PublishedDate);
                paramaters.Add("@Source", posts.Source);
                paramaters.Add("@Status", posts.Status);
                paramaters.Add("@HotDate", posts.HotDate);
                paramaters.Add("@NewDate", posts.NewDate);
                paramaters.Add("@IsActive", posts.IsActive);
                paramaters.Add("@Title", posts.Title);
                paramaters.Add("@contents", posts.Content);
                paramaters.Add("@Description", posts.Description);
                paramaters.Add("@SeoAlias", posts.SeoAlias);
                paramaters.Add("@SeoTitle", posts.SeoTitle);
                paramaters.Add("@SeoMetaKeywords", posts.SeoMetaKeywords);
                paramaters.Add("@SeoMetaDescription", posts.SeoMetaDescription);
                paramaters.Add("@SeoTags", posts.SeoTags);
                paramaters.Add("@LanguageId", posts.LanguageId);                

                paramaters.Add("@UpdateDate", posts.UpdateDate);
                paramaters.Add("@UpdateBy", posts.UpdateBy);
                try
                {
                    await conn.QueryAsync<PostViewModel>(
                        "Update_Posts", paramaters, commandType: CommandType.StoredProcedure);
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
                    await conn.QueryAsync<PostViewModel>(
                       "Delete_Posts_ById", paramaters, commandType: CommandType.StoredProcedure);
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
