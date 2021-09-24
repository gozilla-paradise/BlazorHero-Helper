using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class Request
    {
        public const string TemplateCode = @"namespace $_NAMESPACE_$
{
    public class GetAllPaged$_ENTITY_$sRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}";
    }
}
