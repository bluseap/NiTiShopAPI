using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IProductWholePriceRepository
    {
        Task<List<ProductWholePriceViewModel>> GetListProductWholePrice(long productId);

        Task<bool> CreateProductWholePrice(string productWholePriceXML, string userName, string languageId);
    }
}
