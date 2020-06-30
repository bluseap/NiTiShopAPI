using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class AppUserRolesViewModel
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public string NameRole { get; set; }

        public int StatusRole { get; set; }

        public bool ActiveRole { get; set; }

        public bool ActiveUser { get; set; }





    }
}
