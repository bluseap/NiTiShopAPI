using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NiTiAPI.Dapper.ViewModels;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IAttributeRepository
    {
        Task<List<AttributeViewModel>> GetAll(string culture);

        Task<AttributeViewModel> GetById(int id, string culture);

        Task<List<AttributeViewModel>> GetByCodeSize(string codeSize, string culture);

        Task Add(string culture, AttributeViewModel attribute);

        Task Update(int id, string culture, AttributeViewModel attribute);

        Task Delete(int id);
    }
}
