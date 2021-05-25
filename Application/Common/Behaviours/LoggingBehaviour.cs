using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                                                                                where TResponse : Result
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingBehaviour(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string trackId = Guid.NewGuid().ToString();

            this._httpContextAccessor.HttpContext.Request.Headers["TrackId"] = trackId;

            this._logger.LogInformation($"TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} Request: {JsonConvert.SerializeObject(request)}");

            var response = await next();

            this._logger.LogInformation($"TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} Response: {JsonConvert.SerializeObject(response)}");

            return response;
        }
    }
}
