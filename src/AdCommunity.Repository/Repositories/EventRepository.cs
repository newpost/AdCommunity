﻿using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class EventRepository : GenericRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Event> GetByEventNameAsync(string eventName, CancellationToken? cancellationToken)
    {
        return await _dbContext.Events.FirstOrDefaultAsync(x => x.EventName == eventName, (CancellationToken)(cancellationToken));      
    }
}
