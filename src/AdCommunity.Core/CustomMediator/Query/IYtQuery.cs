﻿using AdCommunity.Core.CustomMediator.Request;

namespace AdCommunity.Core.Extensions.Query;

public interface IYtQuery<out TResponse> : IYtRequest<TResponse>
{
}