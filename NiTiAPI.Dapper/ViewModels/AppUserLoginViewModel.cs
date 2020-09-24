using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NiTiAPI.Dapper.ViewModels
{
    //public static class ConnectionHelper
    //{
    //    public static List<AppUserLoginViewModel> Connections = new List<AppUserLoginViewModel>();
    //}

    public class AppUserLoginViewModel
    {
        public Int64 Id { set; get; }

        public Guid UserId { set; get; }

        public Guid HoSoNhanVienId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        [StringLength(50)]
        public string CorporationId { get; set; }

        [StringLength(500)]
        public string LoginIpAddress { get; set; }

        [StringLength(500)]
        public string LoginIp { get; set; }

        [StringLength(500)]
        public string LoginNameIp { get; set; }

        [StringLength(500)]
        public string LoginIp6Address { get; set; }

        [StringLength(500)]
        public string LoginLocalIp6Adress { get; set; }

        [StringLength(500)]
        public string LoginMacIp { get; set; }

        public int Status { get; set; }

        [StringLength(500)]
        public string StatusContent { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ProviderKey { get; set; }



        public int OnOffId { get; set; }

        public Guid OnOffAppUserId { get; set; }

        public string OnOffUserName { get; set; }

        public bool OnOff { get; set; }

        public DateTime DateOn { get; set; }

        public DateTime DateOff { get; set; }

        public string OnOffMoTa { get; set; }

        public int OnOffStatus { get; set; }


        public ChatRoom ChatRoom { get; set; }
        public string ConnectionId { get; set; }
        public string AvatarUser { get; set; }


        public DateTime CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

    }

    //public enum ChatRoom
    //{
    //    chatroom1,
    //    chatroom2
    //}

}