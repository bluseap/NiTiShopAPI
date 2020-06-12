using System;
using System.Collections.Generic;
using System.Text;

namespace NiTiAPI.Dapper.ViewModels
{
    public class AttributeOptionValueViewModel
    {
        public int Id { get; set; }

        public int OptionId { get; set; }

        public string Value { get; set; }

        public string LanguageId { get; set; }

        public int AttributeId { get; set; }

        public int AttributeOptionSortOrder { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int AttributeSortOrder { get; set; }

        public string BackendType { get; set; }

        public bool IsActive { get; set; }


    }
}
