using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IFunctionRepository
    {
        Task<FunctionViewModel> GetById(string id);

        Task<List<FunctionViewModel>> GetAll(string filter);


       

    }
}
