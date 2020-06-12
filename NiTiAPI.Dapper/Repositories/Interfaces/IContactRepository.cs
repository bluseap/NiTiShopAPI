using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<ContactViewModel> GetById(int id);

        Task<ContactViewModel> GetByCorName(string corporationName);

        Task<ContactViewModel> GetByCorId(int corporationId);

        Task<List<ContactViewModel>> GetListContact();

        Task<PagedResult<ContactViewModel>> GetPaging(string keyword, int coporationId, int pageIndex, int pageSize);

        Task<bool> Create(ContactViewModel contact);

        Task<bool> Update(ContactViewModel contact);      

        Task<bool> Delete(int id, string username);
    }
}
