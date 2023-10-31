﻿namespace AdCommunity.Domain.Entities;

public partial class UserEvent
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? EventId { get; set; }

    public virtual CommunityEvent? Event { get; set; }

    public virtual User? User { get; set; }
}
