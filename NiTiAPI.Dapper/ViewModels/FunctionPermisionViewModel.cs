using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class FunctionPermisionViewModel
    {
        public string Id { get; set; }

        public Guid RoleId { get; set; }

        public string FunctionId { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public bool HasCreated { get; set; }

        public bool HasUpdate { get; set; }

        public bool HasDelete { get; set; }

        public bool HasView { get; set; }

        public bool HasImport { get; set; }

        public bool HasExport { get; set; }

        public bool HasApprove { get; set; }


        public string FunctionPermissionXML { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }
}
