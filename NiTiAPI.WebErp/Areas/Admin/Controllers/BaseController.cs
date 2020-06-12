using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NiTiAPI.Dapper.Repositories.Interfaces;
using System.Net;
using NiTiAPI.Dapper.Models;

namespace NiTiAPI.WebErp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BaseController : Controller
    {
        

    }
}