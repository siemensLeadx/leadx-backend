using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Helpers.Constants;
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
    public class ChangeLeadStatus
    {
        public class Command : IRequest<Result>
        {
            public long lead_id { get; }
            public LeadStatuses status_id { get; }
            public string notes { get; }

            public Command(long lead_id, LeadStatuses status_id, string notes)
            {
                this.lead_id = lead_id;
                this.status_id = status_id;
                this.notes = notes;
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IUnitOfWork _uow;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IApplicationLocalization _localizer;
            private readonly IFirebaseMessageSender _firebaseMessageSender;

            public Handler(IUnitOfWork uow, IAuthenticatedUserService authenticatedUserService,
                IApplicationLocalization localizer, IFirebaseMessageSender firebaseMessageSender)
            {
                _uow = uow;
                _authenticatedUserService = authenticatedUserService;
                _localizer = localizer;
                _firebaseMessageSender = firebaseMessageSender;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var lead = await _uow.Repository<Lead>()
                    .GetByIdAsync(request.lead_id);

                if (!lead.IsLeadRewardSet(request.status_id))
                    return Result.Fail(_localizer.Get(ResourceKeys.SetLeadReward));

                if(!lead.CanPromoteLead(request.status_id))
                    return Result.Fail(_localizer.Get(ResourceKeys.SetApprovedBeforePromoted));

                lead.ChangeStatus(request.status_id,
                    Guid.Parse(_authenticatedUserService.UserId), request.notes);

                if (lead.NotifyUser)
                {
                    var prize = SetLeadRewardPrize(lead, request.status_id);

                    var currentStatus = await _uow.Repository<LeadStatus>()
                        .GetByIdAsync(request.status_id);

                    string messageEn;
                    string messageAr;

                    CreateLocalizedMessage(request, lead, currentStatus, out messageEn, out messageAr);

                    await SendFirebaseMessage(lead, messageEn, messageAr);

                    SaveFirebaseNotification(lead, messageEn, messageAr);
                }

                await _uow.CompleteAsync();

                return Result.Success(_localizer.Get(ResourceKeys.SavedSuccessfully));
            }

            private void SaveFirebaseNotification(Lead lead, string messageEn, string messageAr)
            {
                var newNotification = Notification.CreateNotification(messageEn, messageAr,
                    lead.UserId, lead);

                _uow.Repository<Notification>().Add(newNotification);
            }

            private async Task SendFirebaseMessage(Lead lead, string messageEn, string messageAr)
            {
                var leadUserDevices = _uow.Repository<Device>()
                                        .Entity()
                                        .Where(device => device.UserId == lead.UserId)
                                        .ToList();

                foreach (var device in leadUserDevices)
                {
                    var messageBody = device.DeviceLanguage.Contains(KeyValueConstants.Arabic) ?
                        messageAr : messageEn;

                    var messageData = new Dictionary<string, string>();
                    messageData.Add("lead_id", lead.Id.ToString());

                    // send message to firebase
                    var result = await _firebaseMessageSender.SendToDevice(string.Empty, messageBody,
                        device.Token, messageData);
                }
            }

            private static void CreateLocalizedMessage(Command request, Lead lead, LeadStatus currentStatus, out string messageEn, out string messageAr)
            {
                if (request.status_id == LeadStatuses.Approved || request.status_id == LeadStatuses.Ordered)
                {
                    messageEn = $"Status of Lead with ID {lead.Id}: is {currentStatus.NameEn}\n" +
                        $"you will get a reward in your next payroll";

                    messageAr = $"حالة الفرصة المحتملة رقم {lead.Id}: {currentStatus.NameAr}\n" +
                        $"سوف تحصل على مكافأة مع مرتبك القادم";
                }
                else
                {
                    messageEn = $"Status of Lead with ID {lead.Id}: is {currentStatus.NameEn}";
                    messageAr = $"حالة الفرصة المحتملة رقم {lead.Id}: {currentStatus.NameAr}";
                }
            }

            private RewardPrize SetLeadRewardPrize(Lead lead, LeadStatuses status)
            {
                if ((status == LeadStatuses.Approved || status == LeadStatuses.Ordered) &&
                    lead.RewardCriteriaId.HasValue && 
                    lead.RewardClassId.HasValue)
                {
                    var prize = _uow.Repository<RewardPrize>()
                        .Entity()
                        .FirstOrDefault(prize => prize.RewardClassId == lead.RewardClassId.Value &&
                                                 prize.RewardCriteriaId == lead.RewardCriteriaId.Value);

                    lead.SetRewardPrize(status, prize);

                    return prize;
                }

                return null;
            }         
            


        }
    }
}
