﻿using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.UserEvent.Queries.GetUserEventsQuery;

public class GetUserEventsQueryHandler : IYtRequestHandler<GetUserEventsQuery, List<UserEventDto>>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetUserEventsQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<UserEventDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "userEvents";

        var userEventsDto = await _redisService.GetFromCacheAsync<List<UserEventDto>>(cacheKey);

        if (userEventsDto is null)
        {
            var userEvents = await _unitOfWork.GetRepository<UserEventRepository>().GetAllAsync(null, query => query.Include(x => x.User).Include(x => x.Event), cancellationToken);

            if (userEvents is null || !userEvents.Any())
            {
                throw new NotExistException("User Event",_httpContextAccessor.HttpContext);
            }

            userEventsDto = _mapper.MapList<Domain.Entities.Aggregates.User.UserEvent, UserEventDto>((List<Domain.Entities.Aggregates.User.UserEvent>)userEvents);

            await _redisService.AddToCacheAsync(cacheKey, userEventsDto, CacheTime);
        }

        return userEventsDto;
    }
}
