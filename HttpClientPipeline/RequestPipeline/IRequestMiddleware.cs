using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientPipeline.RequestPipeline
{
    public interface IRequestMiddleware
    {
        public delegate Task<HttpResponseMessage> RequestHandlerDelegate(HttpRequestMessage request,  CancellationToken cancellationToken);

        Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request,  CancellationToken cancellationToken, RequestHandlerDelegate next);
    }
}
