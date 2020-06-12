using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface ICorporationRepository
    {
        Task<CorporationViewModel> GetByOrderId(long orderId);

        Task<CorporationViewModel> GetById(int id);

        Task<List<CorporationViewModel>> GetListCorporations();

        Task<PagedResult<CorporationViewModel>> GetPaging(string keyword, int corporationId, int serviceId, int pageIndex, int pageSize);

        Task<Boolean> Create(CorporationViewModel corporation);

        Task<Boolean> Update(int id, CorporationViewModel corporation);

        Task<Boolean> Delete(int id);
    }
}
