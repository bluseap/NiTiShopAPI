using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NiTiAPI.Dapper.Models;
using NiTiAPI.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NiTiAPI.WebErp.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        UserManager<AppUser> _userManger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options, IConfiguration configuration)
            : base(userManager, roleManager, options)
        {
            _userManger = userManager;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await _userManger.GetRolesAsync(user);
            var permissions = await GetPermissionByUserId(user.Id.ToString());
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                //new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim("FullName", user.FullName),
                new Claim("CorporationId", user.CorporationId.ToString()),
                new Claim("Avatar", user.Avatar ?? string.Empty),
                new Claim("Roles", string.Join(";",roles)),
                new Claim(SystemConstants.UserClaim.Permissions, JsonConvert.SerializeObject(permissions)),

            });
            return principal;
        }

        private async Task<List<string>> GetPermissionByUserId(string userId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                paramaters.Add("@userId", userId);

                var result = await conn.QueryAsync<string>("Get_Permission_ByUserId", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

    }
}
