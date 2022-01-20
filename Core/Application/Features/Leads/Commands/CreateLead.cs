using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Helpers.Interfaces;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Extensions;
using Helpers.Models;
using Helpers.Exceptions;
using Helpers.Constants;

namespace Application.Features.Leads.Commands
{
    public class CreateLead
    {
        public class Command : IRequest<Result>
        {
            public string lead_name { get; set; }
            public string hospital_name { get; set; }
            public string region { get; set; }
            public BusinessOpportunityTypes? business_opportunity_type { get; set; }
            public CustomerStatuses customer_status { get; set; }
            public long customer_due_date { get; set; }
            public string comment { get; set; }
            public string contact_person { get; set; }
            public int[] devices { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.lead_name)
                    .NotEmpty()
                    .MaximumLength(500)
                    .WithName(p => localizer.Get(ResourceKeys.LeadName));

                RuleFor(p => p.hospital_name)
                   .NotEmpty()
                   .MaximumLength(500)
                   .WithName(p => localizer.Get(ResourceKeys.HospitalName));

                RuleFor(p => p.region)
                  .NotEmpty()
                  .MaximumLength(500)
                  .WithName(p => localizer.Get(ResourceKeys.Region));

                RuleFor(p => p.business_opportunity_type)
                 .IsInEnum()
                 .WithName(p => localizer.Get(ResourceKeys.BusinessOpportunityType));

                RuleFor(p => p.customer_status)
                 .NotEmpty()
                 .IsInEnum()
                 .WithName(p => localizer.Get(ResourceKeys.CustomerStatus));

                RuleFor(p => p.customer_due_date)
                 .NotEmpty()
                 .WithName(p => localizer.Get(ResourceKeys.CustomerDueDate));

                RuleFor(p => p.comment)
                  .MaximumLength(2000)
                  .WithName(p => localizer.Get(ResourceKeys.Comment));

                RuleFor(p => p.contact_person)
                   .NotEmpty()
                   .MaximumLength(500)
                   .WithName(p => localizer.Get(ResourceKeys.ContactPerson));

                RuleFor(p => p.devices)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty()
                   .Must(p => p.GroupBy(x => x).All(x => x.Count() == 1))
                   .WithMessage(p => localizer.Get(ResourceKeys.DuplicateDevice))
                   .WithName(p => localizer.Get(ResourceKeys.Devices));
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
                var deviceNotExist = request.devices.Except(_uow.Repository<NeededDevice>()
                    .Entity().Select(d => d.Id)).Any();

                if(deviceNotExist)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                            new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.devices), ResourceKeys.InvalidDeviceType) });

                var newLead = new Lead
                {
                    LeadName = request.lead_name,
                    HospitalName = request.hospital_name,
                    Region = request.region,
                    Comment = request.comment,
                    ContactPerson = request.contact_person,
                    BusinessOpportunityTypeId = request.business_opportunity_type,
                    CustomerStatusId = request.customer_status,
                    CustomerDueDate = request.customer_due_date.FromUnixTimeStamp(),
                    UserId = Guid.Parse(_authenticatedUserService.UserId),
                    LeadNeeds = request.devices.Select(deviceId => new LeadNeed
                    {
                        NeededDeviceId = deviceId
                    }).ToList()
                };

                newLead.ChangeStatus(LeadStatuses.New, Guid.Parse(_authenticatedUserService.UserId));

                _uow.Repository<Lead>().Add(newLead);
                await _uow.CompleteAsync();

                return Result.Success(_localizer.Get(ResourceKeys.SavedSuccessfully));
            }
        }

    }
}
