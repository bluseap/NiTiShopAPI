using NiTiAPI.Dapper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IAppUserLoginRepository
    {
        Task Create(AppUserLogin appuserlogin);     

    }
}
