﻿using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Redis;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Application.Features.Event.Queries.GetEventQuery;

public class GetEventQueryHandler : IYtRequestHandler<GetEventQuery, EventDto>
{
    private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(1);
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetEventQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<EventDto> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"event:{request.Id}";

        var eventDto = await _redisService.GetFromCacheAsync<EventDto>(cacheKey);

        if (eventDto is null)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(request.Id, query=>query.Include(x=>x.Community), cancellationToken);

            if (@event is null)
                throw new NotExistException("Event",_httpContextAccessor.HttpContext);

            eventDto = _mapper.Map<Domain.Entities.Aggregates.Community.Event, EventDto>(@event);

            await _redisService.AddToCacheAsync(cacheKey, eventDto, CacheTime);
        }

        return eventDto;
    }
}
