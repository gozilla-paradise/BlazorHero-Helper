using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class MappingProfile
    {
        public const string TemplateCode = @"using AutoMapper;
using $_ADD_EDIT_CQRS_NAMESPACE_$;
using $_ENTITY_NAMESPACE_$;
using System;

namespace $_NAMESPACE_$
{
    public class $_ENTITY_$Profile : Profile
    {
        public $_ENTITY_$Profile()
        {
            CreateMap<AddEdit$_ENTITY_$Command, $_ENTITY_$>().ReverseMap();
        }
    }
}";
    }
}
