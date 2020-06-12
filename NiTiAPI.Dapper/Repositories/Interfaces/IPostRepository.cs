using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IPostRepository
    {

        #region Posts

        Task<List<PostViewModel>> GetAll(string culture);

        Task<PostViewModel> GetById(int id, string languageId);       

        Task<PagedResult<PostViewModel>> GetPaging(string keyword, string culture, int corporationId, int categoryNewsId, int pageIndex, int pageSize);
              
        Task<bool> Create(PostViewModel posts);

        Task<bool> CreatePostImageXML(PostViewModel posts, string listImageXML);

        Task<bool> Update(PostViewModel posts);

        Task<bool> Delete(int id, string username);      


        #endregion Posts

    }
}
