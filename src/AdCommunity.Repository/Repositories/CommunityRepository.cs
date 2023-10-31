﻿using AdCommunity.Domain.Entities;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class CommunityRepository : GenericRepository<Community>, ICommunityRepository   
{
    public CommunityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
