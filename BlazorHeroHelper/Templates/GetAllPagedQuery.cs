using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class GetAllPagedQuery
    {
        public const string TemplateCode = @"using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using MediatR;
using $_EXTENSION_NAMESPACE_$;
using $_INTERFACE_REPOSITORY_NAMESPACE_$;
using $_FILTER_SPECIFICATION_NAMESPACE_$;
using $_ENTITY_NAMESPACE_$;
using $_SHARED_WRAPPER_NAMESPACE_$;
using System;

namespace $_NAMESPACE_$
{
    public class GetAll$_ENTITY_$sQuery : IRequest<PaginatedResult<GetAllPaged$_ENTITY_$sResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[]
            OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAll$_ENTITY_$sQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class
        GetAll$_ENTITY_$sQueryHandler : IRequestHandler<GetAll$_ENTITY_$sQuery, PaginatedResult<GetAllPaged$_ENTITY_$sResponse>>
    {
        private readonly IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> _unitOfWork;

        public GetAll$_ENTITY_$sQueryHandler(IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPaged$_ENTITY_$sResponse>> Handle(GetAll$_ENTITY_$sQuery request,
            CancellationToken cancellationToken)
        {
            Expression<Func<$_ENTITY_$, GetAllPaged$_ENTITY_$sResponse>> expression = e => new GetAllPaged$_ENTITY_$sResponse
            {
                Id = e.Id,
                //TODO: Insert Data Member Here
            };
            var recordFilterSpec = new $_ENTITY_$FilterSpecification(request.SearchString);
            
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<$_ENTITY_$>().Entities
                    .Specify(recordFilterSpec)
                    .Select(expression)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join("","", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<$_ENTITY_$>().Entities
                    .Specify(recordFilterSpec)
                    .OrderBy(ordering) // require system.linq.dynamic.core
                    .Select(expression)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
        }
    }
}";
    }
}
