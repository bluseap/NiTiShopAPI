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
    public class PostsImagesRepository : IPostsImagesRepository
    {
        private readonly string _connectionString;

        public PostsImagesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<List<PostsImagesViewModel>> GetListPostImages(int postId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();
                paramaters.Add("@postId", postId);

                var result = await conn.QueryAsync<PostsImagesViewModel>("Get_PostsImages_ByPostId",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<bool> PostImages(int postId, string images, string userName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@postId", postId);
                paramaters.Add("@images", images);

                paramaters.Add("@CreateBy", userName);
                try
                {
                    await conn.QueryAsync<PostsImagesViewModel>(
                        "Create_PostsImages", paramaters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> DeleteImage(long postsImageId, string userName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var paramaters = new DynamicParameters();

                paramaters.Add("@Id", postsImageId);
                paramaters.Add("@CreateBy", userName);

                try
                {
                    await conn.QueryAsync<PostsImagesViewModel>(
                        "Delete_PostsImages_ById", paramaters, commandType: CommandType.StoredProcedure);
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
