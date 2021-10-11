using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class Route
    {
        public const string TemplateCode = @"using System.Linq;
using System;

namespace $_NAMESPACE_$
{
    public static class $_ENTITY_$sEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $""api/v1/$_ENTITY_LOWER_$s?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy="";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $""{orderByPart},"";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string GetById($_DEFAULT_ID_DATATYPE_$ $_ENTITY_LOWER_$Id)
        {
            return $""api/v1/$_ENTITY_LOWER_$s/{$_ENTITY_LOWER_$Id}"";
        }

        public static string GetCount = ""api/v1/$_ENTITY_LOWER_$s/count"";

        public static string Save = ""api/v1/$_ENTITY_LOWER_$s"";
        public static string Delete = ""api/v1/$_ENTITY_LOWER_$s"";
    }
}";
    }
}
