using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class Entity
    {
        public const string TemplateCode = @"using $_CONTRACTS_$;
using $_ENUMS_$;

namespace $_NAMESPACE_$
{
    public class $_ENTITY_$ : AuditableEntity<int>
    {
        //TODO: Insert Data Member Here
    }
}";

    }
}
