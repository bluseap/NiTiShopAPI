using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser() { }
        public AppUser(Guid id, string fullName, string username,
            string email, string phoneNumber, string avatar
            //, Status status
            , int corporationId, int status, bool active, int sortOrder,
            DateTime createDate, string createby)
        {
            Id = id;
            FullName = fullName;
            UserName = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Avatar = avatar;
            //Status = status;
            CorporationId = corporationId;
            Status = status;
            Active = active;
            SortOrder = sortOrder;
            CreateDate = createDate;
            CreateBy = createby;
        }

        //public Guid Id { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }


        public int CorporationId { get; set; }

        //public string UserName { get; set; }

        public string Avatar { get; set; }

        public string NormalizedUserName { get; set; }

        //public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { set; get; }

        public string FullName { get; set; }

        public string Adress { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

    }

   

}
