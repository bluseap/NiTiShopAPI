using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IAttributeOptionValueRepository
    {
        Task<List<AttributeOptionValueViewModel>> GetListByAttribute(int attributeId, string language);

        Task<List<AttributeOptionValueViewModel>> GetListByAttributeSize(long productId, string language);

        Task<AttributeOptionValueViewModel> GetByProductId(long productId, string language);

        Task<List<AttributeOptionValueViewModel>> GetByAttriCodeSize(string codeSize, string language);

        Task<AttributeOptionValueViewModel> GetById(int attributeOptionValueId);

        Task<List<AttributeOptionValueViewModel>> GetListAll();
    }
}
