using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class AddEditCommand
    {
        public const string TemplateCode = @"using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using $_ENTITY_NAMESPACE_$;
using $_INTERFACE_REPOSITORY_NAMESPACE_$;
using $_SHARED_WRAPPER_NAMESPACE_$;

namespace $_NAMESPACE_$
{
    public partial class AddEdit$_ENTITY_$Command : IRequest<Result<$_DEFAULT_ID_DATATYPE_$>>
    {
        public $_DEFAULT_ID_DATATYPE_$ Id { get; set; }
        //TODO: Insert Data Member Here
    }

    internal class AddEdit$_ENTITY_$CommandHandler : IRequestHandler<AddEdit$_ENTITY_$Command, Result<$_DEFAULT_ID_DATATYPE_$>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> _unitOfWork;
        private readonly IStringLocalizer<AddEdit$_ENTITY_$CommandHandler> _localizer;

        public AddEdit$_ENTITY_$CommandHandler(IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> unitOfWork, IMapper mapper,
            IStringLocalizer<AddEdit$_ENTITY_$CommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<$_DEFAULT_ID_DATATYPE_$>> Handle(AddEdit$_ENTITY_$Command command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<$_ENTITY_$>().Entities.Where(s => s.Id != command.Id).AnyAsync(cancellationToken: cancellationToken))
            {
                return await Result<$_DEFAULT_ID_DATATYPE_$>.FailAsync(""Record is already existed"");
            }

            if (command.Id == 0)
            {
                var record = _mapper.Map<$_ENTITY_$>(command);
                await _unitOfWork.Repository<$_ENTITY_$>().AddAsync(record);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<$_DEFAULT_ID_DATATYPE_$>.SuccessAsync(record.Id, ""Create Record Successfully"");
            }
            else
            {
                var record = await _unitOfWork.Repository<$_ENTITY_$>()
                    .GetByIdAsync(command.Id);
                if (record != null)
                {

                    //TODO: Modify And Edit Record Here
                    /* 

                    record.IsShow = command.IsShow;
                    record.MinimumLimit = command.MinimumLimit;
                    record.MaximumLimit = command.MaximumLimit;

                    */

                    await _unitOfWork.Repository<$_ENTITY_$>().UpdateAsync(record);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<$_DEFAULT_ID_DATATYPE_$>.SuccessAsync(record.Id, ""Update Record Successfully"");
                }
                else
                {
                    return await Result<$_DEFAULT_ID_DATATYPE_$>.FailAsync(""Record Not Found"");
                }
            }
        }
    }
}";
    }
}
