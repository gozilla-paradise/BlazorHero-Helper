using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class DeleteCommand
    {
        public const string TemplateCode = @"using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using $_INTERFACE_REPOSITORY_NAMESPACE_$;
using $_SHARED_WRAPPER_NAMESPACE_$;
using $_ENTITY_NAMESPACE_$;
using System;

namespace $_NAMESPACE_$
{
    public partial class Delete$_ENTITY_$Command : IRequest<Result<$_DEFAULT_ID_DATATYPE_$>>
    {
        public $_DEFAULT_ID_DATATYPE_$ Id { get; set; }
    }

    internal class Delete$_ENTITY_$CommandHandler : IRequestHandler<Delete$_ENTITY_$Command, Result<$_DEFAULT_ID_DATATYPE_$>>
    {
        private readonly IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> _unitOfWork;
        private readonly IStringLocalizer<Delete$_ENTITY_$CommandHandler> _localizer;

        public Delete$_ENTITY_$CommandHandler(IUnitOfWork<$_DEFAULT_ID_DATATYPE_$> unitOfWork,
            IStringLocalizer<Delete$_ENTITY_$CommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<$_DEFAULT_ID_DATATYPE_$>> Handle(Delete$_ENTITY_$Command command, CancellationToken cancellationToken)
        {
            var record =
                await _unitOfWork.Repository<$_ENTITY_$>().GetByIdAsync(command.Id);
            if (record != null)
            {
                await _unitOfWork.Repository<$_ENTITY_$>().DeleteAsync(record);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<$_DEFAULT_ID_DATATYPE_$>.SuccessAsync(record.Id, ""Delete Record Successfully"");
            }
            else
            {
                return await Result<$_DEFAULT_ID_DATATYPE_$>.FailAsync(""Record Not Found"");
            }
        }
    }
}";
    }
}
