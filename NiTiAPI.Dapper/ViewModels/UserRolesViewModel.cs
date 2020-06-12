using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class UserRolesViewModel
    {           

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public string Name { get; set; }

        public string RoleDescription { get; set; }
    }
}
