using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserViewModel> GetById(Guid id);

        Task<List<UserViewModel>> GetListUser();

        Task<PagedResult<UserViewModel>> GetPaging(string keyword, int coporationId, int pageIndex, int pageSize);

        Task<bool> CreateUser(UserViewModel userVm);

        Task<bool> UpdateUser(UserViewModel userVm, string roles);

        Task<bool> UpdateUserPass(UserViewModel userVm);

        Task<bool> Delete(Guid id, string username);

    }
}
