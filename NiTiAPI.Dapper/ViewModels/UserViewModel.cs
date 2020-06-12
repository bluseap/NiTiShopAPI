using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public static class ConnectionHelper
    {
        public static List<UserViewModel> Connections = new List<UserViewModel>();
    }

    public enum ChatRoom
    {
        chatroom1,
        chatroom2
    }

    public class UserViewModel
    {       
        public Guid Id { get; set; }

        public int CorporationId { get; set; }

        public string CorporationName { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }


        public ChatRoom ChatRoom { get; set; }

        public string ConnectionId { get; set; }
    }
}
