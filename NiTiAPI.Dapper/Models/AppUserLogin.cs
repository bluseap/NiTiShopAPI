using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NiTiAPI.Dapper.Models
{   

    public class AppUserLogin
    {
        public long Id { set; get; }

        public string CorporationId { get; set; }

        public Guid UserId { set; get; }

        public string UserName { get; set; }

        public string FullName { get; set; }      
       
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
        
        [StringLength(500)]
        public string StatusContent { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ProviderKey { get; set; }

        public int Status { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }


    }
    
}
