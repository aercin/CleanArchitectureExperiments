using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                                                                                           where TResponse : Result
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"(TrackId: {_httpContextAccessor.HttpContext.Request.Headers["TrackId"]}) Request throw an unhandled exception");
                throw;
            }
        }
    }
}
