using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Repositories.Interfaces
{
    public interface IAppUserLoginRepository
    {
        Task Create(AppUserLogin appuserlogin);

        Task<Boolean> AppUserLoginAUD(AppUserLoginViewModel appuser, string parameters);

    }
}
