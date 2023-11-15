﻿using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Features.Community.Commands.CreateCommunityCommand;
using AdCommunity.Application.Features.Community.Commands.DeleteCommunityCommand;
using AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;
using AdCommunity.Application.Features.Community.Queries.GetCommunitiesQuery;
using AdCommunity.Application.Features.Community.Queries.GetCommunityQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public CommunitiesController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{communityId}")]
        public async Task<CommunityDto> Get(int communityId)
        {
            GetCommunityQuery query = new GetCommunityQuery { Id = communityId };
            CommunityDto community = await _mediator.Send(query);

            return community;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CommunityDto>> GetAll()
        {
            GetCommunitiesQuery query = new GetCommunitiesQuery();
            IEnumerable<CommunityDto> communities = await _mediator.Send(query);

            return communities;
        }

        [HttpPost("[action]")]
        public async Task<CommunityCreateDto> Create(CommunityCreateDto community)
        {
            CreateCommunityCommand command = new CreateCommunityCommand(community.Name, community.Description, community.Tags, community.Location, community.Website, community.Facebook, community.Twitter, community.Instagram, community.Github, community.Medium,community.UserId);
            CommunityCreateDto createdCommunity = await _mediator.Send(command);

            return createdCommunity;
        }

        [HttpDelete("[action]/{communityId}")]
        public async Task<bool> Delete(int communityId)
        {
            DeleteCommunityCommand command = new DeleteCommunityCommand { Id = communityId };
            bool deletedCommunity = await _mediator.Send(command);

            return deletedCommunity;
        }


        [HttpPut("[action]")]
        public async Task<bool> Update(CommunityUpdateDto community)
        {
            UpdateCommunityCommand command = new UpdateCommunityCommand(community.Id, community.Name, community.Description, community.Tags, community.Location, community.Website, community.Facebook, community.Twitter, community.Instagram, community.Github, community.Medium,community.UserId);
            bool updatedCommunity = await _mediator.Send(command);

            return updatedCommunity;
        }
    }
}
