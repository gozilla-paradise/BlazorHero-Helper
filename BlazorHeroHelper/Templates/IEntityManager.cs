using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class IEntityManager
    {
        public const string TemplateCode1 = @"using $_ADD_EDIT_CQRS_NAMESPACE_$;
using $_GET_ALL_PAGED_CQRS_NAMESPACE_$;
using $_GET_MODEL_BY_ID_CQRS_NAMESPACE_$;
using $_REQUEST_NAMESPACE_$;
using $_SHARED_WRAPPER_NAMESPACE_$;
using System.Threading.Tasks;

namespace $_NAMESPACE_$
{
    public interface I$_ENTITY_$Manager : IManager
    {
        Task<PaginatedResult<GetAllPaged$_ENTITY_$sResponse>> Get$_ENTITY_$sAsync(GetAllPaged$_ENTITY_$sRequest request);

        Task<IResult<Get$_ENTITY_$ByIdResponse>> GetByIdAsync(Get$_ENTITY_$ByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEdit$_ENTITY_$Command request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}";

        public const string TemplateCode2 = @"using $_ADD_EDIT_CQRS_NAMESPACE_$;
using $_GET_ALL_PAGED_CQRS_NAMESPACE_$;
using $_GET_MODEL_BY_ID_CQRS_NAMESPACE_$;
using $_REQUEST_NAMESPACE_$;
using $_CLIENT_INFRA_EXTENSION_NAMESPACE_$;
using $_SHARED_WRAPPER_NAMESPACE_$;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace $_NAMESPACE_$
{
    public class $_ENTITY_$Manager : I$_ENTITY_$Manager
    {
        private readonly HttpClient _httpClient;

        public $_ENTITY_$Manager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($""{Routes.$_ENTITY_$sEndpoints.Delete}/{id}"");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllPaged$_ENTITY_$sResponse>> Get$_ENTITY_$sAsync(GetAllPaged$_ENTITY_$sRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.$_ENTITY_$sEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPaged$_ENTITY_$sResponse>();
        }

        public async Task<IResult<Get$_ENTITY_$ByIdResponse>> GetByIdAsync(Get$_ENTITY_$ByIdQuery request)
        {
            var response = await _httpClient.GetAsync(Routes.$_ENTITY_$sEndpoints.GetById(request.Id));
            return await response.ToResult<Get$_ENTITY_$ByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEdit$_ENTITY_$Command request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.$_ENTITY_$sEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}";
    }
}
