using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class GetModelByIdResponse
    {
        public const string TemplateCode = @"using $_ENUM_NAMESPACE_$;

namespace $_NAMESPACE_$
{
    public class Get$_ENTITY_$ByIdResponse
    {
        public int Id { get; set; }
        //TODO: Insert Data Member Here
    }
}
";
    }
}
