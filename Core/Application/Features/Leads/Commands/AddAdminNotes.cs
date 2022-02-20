using Application.Interfaces;
using Domain.Entities;
using Helpers.Interfaces;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Leads.Commands
{
    public class AddAdminNotes
    {
        public class Command : IRequest<Result>
        {
            public long lead_id { get; }
            public string admin_notes { get; }

            public Command(long lead_id, string admin_notes)
            {
                this.lead_id = lead_id;
                this.admin_notes = admin_notes;
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow,
                IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var lead = await _uow.Repository<Lead>()
                    .GetByIdAsync(request.lead_id);

                lead.AdminNotes = request.admin_notes;

                await _uow.CompleteAsync();

                return Result.Success(_localizer.Get(ResourceKeys.SavedSuccessfully));
            }

        }
    }
}
