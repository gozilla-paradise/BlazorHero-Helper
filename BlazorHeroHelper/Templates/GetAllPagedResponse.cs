using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class GetAllPagedResponse
    {
        public const string TemplateCode = @"using $_ENUM_NAMESPACE_$;

namespace $_NAMESPACE_$
{
    public class GetAllPaged$_ENTITY_$sResponse
    {
        public int Id { get; set; }
    }
}
";
    }
}
