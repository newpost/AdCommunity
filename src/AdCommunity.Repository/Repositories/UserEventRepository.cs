﻿using AdCommunity.Domain.Entities;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class UserEventRepository : GenericRepository<UserEvent>, IUserEventRepository
{
    public UserEventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
