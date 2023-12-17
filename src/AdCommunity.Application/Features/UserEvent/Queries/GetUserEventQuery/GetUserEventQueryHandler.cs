﻿using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserEvent.Queries.GetUserEventQuery;

public class GetUserEventQueryHandler : IYtRequestHandler<GetUserEventQuery, UserEventDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetUserEventQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserEventDto> Handle(GetUserEventQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"userEvent:{request.Id}";

        var userEventDto = await _redisService.GetFromCacheAsync<UserEventDto>(cacheKey);

        if (userEventDto is null)
        {
            var userEvent = await _unitOfWork.GetRepository<UserEventRepository>().GetAsync(request.Id, query => query.Include(x => x.User).Include(x => x.Event), cancellationToken);

            if (userEvent is null)
                throw new NotExistException("User Event",_httpContextAccessor.HttpContext);

            userEventDto = _mapper.Map<Domain.Entities.Aggregates.User.UserEvent, UserEventDto>(userEvent);

            await _redisService.AddToCacheAsync(cacheKey, userEventDto, CacheTime);
        }

        return userEventDto;
    }
}
