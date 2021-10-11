using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class IRepository
    {
        public const string TemplateCode = @"using $_CONTRACTS_$;
using System;

namespace $_NAMESPACE_$
{
    public class I$_MODEL_$Repository : AuditableEntity<$_DEFAULT_ID_DATATYPE_$>
    {
        
    }
}
";
    }
}
