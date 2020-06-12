using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IUserOnlineRepository
    {
        Task<bool> AddUserOnline(int corporationId, int CountUser, string notes);

        Task<List<UserOnlineViewModel>> GetListCorporation(int Id);
    }
}
