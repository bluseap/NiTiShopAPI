using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface ICategoryNewsRepository
    {
        Task<CategoryNewsViewModel> GetById(int id);

        Task<List<CategoryNewsViewModel>> GetListCategory();

        Task<List<CategoryNewsViewModel>> GetListCateByCor(string corporationName);

        List<CategoryNewsViewModel> GetListCateByCorId(int corporationId);

        List<CategoryNewsViewModel> GetListHomeCateByCorLangId(int corporationId, string languageId);

        Task<PagedResult<CategoryNewsViewModel>> GetPaging(string keyword, int coporationId, int pageIndex, int pageSize);

        Task<bool> Create(CategoryNewsViewModel category);

        Task<bool> Update(CategoryNewsViewModel category);

        Task<bool> UpdateParent(int fromParent, int toParent, int parameter, string username);

        Task<bool> Delete(int id, string username);
    }
}
