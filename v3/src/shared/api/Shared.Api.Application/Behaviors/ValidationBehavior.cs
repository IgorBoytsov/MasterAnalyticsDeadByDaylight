﻿using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using DBDAnalytics.Shared.Domain.Results;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Shared.Api.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            var validationFailures = 
                (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (validationFailures.Any())
            {
                var errors = validationFailures.Select(f => new Error(ErrorCode.Validation, f.ErrorMessage)).ToList();

                var resultType = typeof(TResponse);

                MethodInfo? failureMethod = resultType.GetMethod(
                    nameof(Result.Failure),
                    BindingFlags.Public | BindingFlags.Static,
                    [typeof(IEnumerable<Error>)]);

                GuardException.Against.That(failureMethod is null, () => new InvalidOperationException($"Не удалось найти статический метод Failure в типе {resultType.Name}"));

                return (TResponse)failureMethod!.Invoke(null, [errors])!;
            }

            return await next(cancellationToken);
        }
    }
}