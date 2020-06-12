using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IUserRolesRepository
    {
        Task<List<UserRolesViewModel>> GetListUser(Guid userid);

        Task<List<UserRolesViewModel>> GetListRole(Guid roleid);

        Task<UserRolesViewModel> GetUserRole(Guid userid, Guid roleid);

        Task<ListTResult<UserRolesViewModel>> GetListUserRole(Guid userId);

        //Task<Boolean> Create(UserRolesViewModel corporation);

        //Task<Boolean> Update(UserRolesViewModel corporation);

    }
}
