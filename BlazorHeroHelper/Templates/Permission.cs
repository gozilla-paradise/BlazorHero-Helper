using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class Permission
    {
        public const string TemplateCode = @"[DisplayName(""$_ENTITY_$s"")]
        [Description(""$_ENTITY_$s Permissions"")]
        public static class $_ENTITY_$s
        {
            public const string View = ""Permissions.$_ENTITY_$s.View"";
            public const string Create = ""Permissions.$_ENTITY_$s.Create"";
            public const string Edit = ""Permissions.$_ENTITY_$s.Edit"";
            public const string Delete = ""Permissions.$_ENTITY_$s.Delete"";
            public const string Search = ""Permissions.$_ENTITY_$s.Search"";
        }";
    }
}
