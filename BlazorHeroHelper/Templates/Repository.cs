using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class Repository
    {
        public const string TemplateCode = @"using $_INTERFACE_REPOSITORY_NAMESPACE_$;
using $_ENTITY_NAMESPACE_$;
using System;

namespace $_NAMESPACE_$
{
    public class $_ENTITY_$Repository : I$_ENTITY_$Repository
    {
        private readonly IRepositoryAsync<$_ENTITY_$, $_DEFAULT_ID_DATATYPE_$> _repository;

        public $_ENTITY_$Repository(IRepositoryAsync<$_ENTITY_$, $_DEFAULT_ID_DATATYPE_$> repository)
        {
            _repository = repository;
        }
    }
}";
    }
}
