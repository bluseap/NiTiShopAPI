using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using NiTiAPI.Dapper.Models;
using NiTiAPI.Dapper.Repositories.Interfaces;
using NiTiAPI.Dapper.ViewModels;
using NiTiAPI.Utilities.Constants;

using LuckySo.WebAPI.Filters;
using LuckySo.WebAPI.Helpers;

namespace LuckySo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AppUserRolesController : ControllerBase
    {      

        private readonly ILogger<AppUserRolesController> _logger;

        IAppUserRolesRepository _appuserrolesRepository;

        public AppUserRolesController( ILogger<AppUserRolesController> logger, IAppUserRolesRepository appuserrolesRepository)
        {            
            _logger = logger;
            _appuserrolesRepository = appuserrolesRepository;
        }

        [HttpGet("GetByCor", Name = "GetByCorporationId")]
        public async Task<List<AppUserRolesViewModel>> GetByCorporationId(int coporationid)
        {
            return await _appuserrolesRepository.GetListpUserRoles(coporationid);
        }


    }
}