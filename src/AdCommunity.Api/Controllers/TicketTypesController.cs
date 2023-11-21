﻿using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;
using AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;
using AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;
using AdCommunity.Application.Features.Ticket.Queries.GetTicketQuery;
using AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public TicketTypesController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{ticketId}")]
        public async Task<TicketTypesDto> Get(int ticketId)
        {
            GetTicketTypeQuery query = new GetTicketTypeQuery { Id = ticketId };
            TicketTypesDto ticket = await _mediator.Send(query);

            return ticket;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<TicketTypesDto>> GetAll()
        {
            GetTicketTypesQuery query = new GetTicketTypesQuery();
            IEnumerable<TicketTypesDto> ticketTypes = await _mediator.Send(query);

            return ticketTypes;
        }

        [HttpPost("[action]")]
        public async Task<TicketTypesCreateDto> Create(TicketTypesCreateDto ticket)
        {
            CreateTicketTypesCommand command = new CreateTicketTypesCommand(ticket.CommunityEventId,ticket.CommunityId,ticket.Price);

            TicketTypesCreateDto createdTicket = await _mediator.Send(command);

            return createdTicket;
        }

        [HttpDelete("[action]/{ticketId}")]
        public async Task<bool> Delete(int ticketId)
        {
            DeleteTicketCommand command = new DeleteTicketCommand { Id = ticketId };
            bool deletedTicket = await _mediator.Send(command);

            return deletedTicket;
        }


        [HttpPut("[action]")]
        public async Task<bool> Update(TicketTypesUpdateDto ticket)
        {
            UpdateTicketTypeCommand command = new UpdateTicketTypeCommand(ticket.Id,ticket.CommunityEventId,ticket.CommunityId,ticket.Price);
            bool updatedTicket = await _mediator.Send(command);

            return updatedTicket;
        }
    }
}
