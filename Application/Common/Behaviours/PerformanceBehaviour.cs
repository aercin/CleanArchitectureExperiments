using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                                                                                    where TResponse : Result
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ITimer _timer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PerformanceBehaviour(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor, ITimer timer)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
            this._timer = timer;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            this._timer.Start();

            var response = await next();

            var elapsedMilliseconds =  this._timer.Stop();
             
            if (elapsedMilliseconds > 500)
            {
                this._logger.LogWarning($"Long Running Request TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} and ElapsedMilliseconds: {elapsedMilliseconds}");
            }

            return response;
        }
    }
}
