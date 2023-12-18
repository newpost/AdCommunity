﻿using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;

public class UpdateCommunityCommandHandler : IYtRequestHandler<UpdateCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;

    public UpdateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<bool> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingCommunity is null)
            throw new NotExistException(_localizationService,"Community");

        existingCommunity.SetDate();

        var user= await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);
        
        if (user is null)
            throw new NotExistException(_localizationService, "User");

        existingCommunity.AssignUser(user);

        _mapper.Map(request, existingCommunity);

        _unitOfWork.GetRepository<CommunityRepository>().Update(existingCommunity);

        _rabbitMqFactory.PublishMessage("update_community_queue", $"Community with Id: {existingCommunity.Id}  has been edited.");

        return true;
    }
}
