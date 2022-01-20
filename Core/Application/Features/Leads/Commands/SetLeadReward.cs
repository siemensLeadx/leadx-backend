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
    public class SetLeadReward
    {
        public class Command: IRequest<Result>
        {
            public long lead_id { get; }
            public int? reward_class_id { get; }
            public int? reward_criteria_id { get; }

            public Command(long lead_id, int? reward_class_id, int? reward_criteria_id)
            {
                this.lead_id = lead_id;
                this.reward_class_id = reward_class_id;
                this.reward_criteria_id = reward_criteria_id;
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IUnitOfWork _uow;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IAuthenticatedUserService authenticatedUserService,
                IApplicationLocalization localizer)
            {
                _uow = uow;
                _authenticatedUserService = authenticatedUserService;
                _localizer = localizer;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var lead = await _uow.Repository<Lead>().GetByIdAsync(request.lead_id);

                lead.RewardClassId = request.reward_class_id;
                lead.RewardCriteriaId = request.reward_criteria_id;

                await _uow.CompleteAsync();

                return Result.Success(_localizer.Get(ResourceKeys.SavedSuccessfully));
            }
        }
    }
}
