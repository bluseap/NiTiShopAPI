using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Utilities.Dtos
{
    public class ListTResult<T>
    {
        public List<T> Items { set; get; }

        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int CorporationId { get; set; }

        public int Status { get; set; }

        public string Avatar { get; set; }

    }
}
