using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class GetModelByIdQuery
    {
        public const string TemplateCode = @"using AutoMapper;
using $_INTERFACE_REPOSITORY_NAMESPACE_$;
using $_ENTITY_NAMESPACE_$;
using $_SHARED_WRAPPER_NAMESPACE_$;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace $_NAMESPACE_$
{
    public class Get$_ENTITY_$ByIdQuery : IRequest<Result<Get$_ENTITY_$ByIdResponse>>
    {
        public $_DEFAULT_ID_DATATYPE_$ Id { get; set; }
    }

    internal class Get$_ENTITY_$ByIdQueryHandler : IRequestHandler<Get$_ENTITY_$ByIdQuery, Result<Get$_ENTITY_$ByIdResponse>>
    {
        private readonly IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> _unitOfWork;
        private readonly IMapper _mapper;

        public Get$_ENTITY_$ByIdQueryHandler(IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Get$_ENTITY_$ByIdResponse>> Handle(Get$_ENTITY_$ByIdQuery query, CancellationToken cancellationToken)
        {
            var $_ENTITY_LOWER_$ = await _unitOfWork.Repository<$_ENTITY_$>().GetByIdAsync(query.Id);
            var mapped$_ENTITY_$ = _mapper.Map<Get$_ENTITY_$ByIdResponse>($_ENTITY_LOWER_$);
            return await Result<Get$_ENTITY_$ByIdResponse>.SuccessAsync(mapped$_ENTITY_$);
        }
    }
}";
    }
}
