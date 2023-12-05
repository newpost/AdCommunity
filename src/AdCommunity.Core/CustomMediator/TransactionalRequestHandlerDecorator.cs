﻿using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Core.CustomMediator;

public class TransactionalRequestHandlerDecorator<TRequest, TResponse> : IYtRequestHandler<TRequest, TResponse>
    where TRequest : IYtRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtRequestHandler<TRequest, TResponse> _innerHandler;

    public TransactionalRequestHandlerDecorator(IUnitOfWork unitOfWork, IYtRequestHandler<TRequest, TResponse> innerHandler)
    {
        _unitOfWork = unitOfWork;
        _innerHandler = innerHandler;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        // Belirli bir dependency'nin sonunda "CommandHandler" kelimesi geçiyorsa işlem yap
        if (_innerHandler.GetType().Name.EndsWith("CommandHandler"))
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await _innerHandler.Handle(request, cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        else
        {
            // Belirli bir türde değilse, normal işlemi yap
            return await _innerHandler.Handle(request, cancellationToken);
        }
    }
}
