using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NiTiAPI.Dapper.Models
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base()
        {

        }
        public AppRole(string name, string description) : base(name)
        {
            this.Description = description;
        }
        [StringLength(250)]
        public string Description { get; set; }



        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
