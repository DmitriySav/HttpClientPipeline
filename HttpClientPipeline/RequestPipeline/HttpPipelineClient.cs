using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientPipeline.RequestPipeline
{
    public class HttpPipelineClient : HttpClient
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpPipeline _httpPipeline;

        public HttpPipelineClient(IServiceProvider serviceProvider, HttpPipeline httpPipeline)
        {
            _serviceProvider = serviceProvider;
            _httpPipeline = httpPipeline;
        }
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is PipelineRequest)
                return await base.SendAsync(request, cancellationToken);

            return await _httpPipeline.Handle(request, cancellationToken, _serviceProvider);
        }
    }
}
