using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderViewModel> GetById(long id);
       
        Task<PagedResult<OrderViewModel>> GetAllPagingOrder(int corporationId,
            string keyword, int pageIndex, int pageSize);

        Task<bool> CreateOrder(string orderXML, string CreateBy);

        Task<bool> CreateOrderCorpoId(string orderXML, string CreateBy);

        Task<bool> UpdateOrder(OrderViewModel order);

        Task<bool> DeleteOrder(long id, string username);

    }
}
