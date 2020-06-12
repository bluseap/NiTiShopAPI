using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IProductQuantitiesRepository
    {
        Task<List<ProductQuantitiesViewModel>> GetListProductQuantities(long productId);

        Task<bool> CreateProductQuantities(string productQuantiesXML, string userName, string languageId);

        Task<bool> DeleteQuantities(long productQuantitiesId, string userName);

    }
}
