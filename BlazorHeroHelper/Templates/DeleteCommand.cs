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

namespace $_NAMESPACE_$
{
    public partial class Delete$_ENTITY_$Command : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class Delete$_ENTITY_$CommandHandler : IRequestHandler<Delete$_ENTITY_$Command, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<Delete$_ENTITY_$CommandHandler> _localizer;

        public Delete$_ENTITY_$CommandHandler(IUnitOfWork<int> unitOfWork,
            IStringLocalizer<Delete$_ENTITY_$CommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(Delete$_ENTITY_$Command command, CancellationToken cancellationToken)
        {
            var record =
                await _unitOfWork.Repository<$_ENTITY_$>().GetByIdAsync(command.Id);
            if (record != null)
            {
                await _unitOfWork.Repository<$_ENTITY_$>().DeleteAsync(record);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(record.Id, ""Delete Record Successfully"");
            }
            else
            {
                return await Result<int>.FailAsync(""Record Not Found"");
            }
        }
    }
}";
    }
}
