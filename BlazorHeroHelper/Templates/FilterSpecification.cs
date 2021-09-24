using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class FilterSpecification
    {
        public const string TemplateCode = @"using $_SPECIFICATION_BASE_NAMESPACE_$;
using $_ENTITY_NAMESPACE_$;

namespace $_NAMESPACE_$
{
    public class $_ENTITY_$FilterSpecification :  HeroSpecification<$_ENTITY_$>
    {
        public $_ENTITY_$FilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                //Criteria = p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString);
            }
        }
    }
}";
    }
}
