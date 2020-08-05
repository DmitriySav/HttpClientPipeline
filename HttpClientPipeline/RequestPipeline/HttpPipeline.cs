using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientPipeline.RequestPipeline
{
    public class HttpPipeline
    {
        public Task<object> Handle(object request,  CancellationToken cancellationToken,
            IServiceProvider serviceProvider)
        {
            return Handle((HttpRequestMessage)request, cancellationToken, serviceProvider)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        ExceptionDispatchInfo.Capture(t.Exception.InnerException).Throw();
                    }

                    return (object)t.Result;
                }, cancellationToken);
        }

        public Task<HttpResponseMessage> Handle(HttpRequestMessage request, CancellationToken cancellationToken, IServiceProvider serviceProvider)
        {
            Task<HttpResponseMessage> Handler(HttpRequestMessage r, CancellationToken ct) => ((HttpPipelineClient)serviceProvider.GetService(typeof(HttpPipelineClient))).SendAsync(new PipelineRequest(request), cancellationToken);

            return ((IEnumerable<IRequestMiddleware>)serviceProvider.GetService(typeof(IEnumerable<IRequestMiddleware>)))
                .Reverse()
                .Aggregate((IRequestMiddleware.RequestHandlerDelegate)Handler,
                    (next, pipeline) => (r,  ct) => pipeline.HandleAsync((PipelineRequest)r,  ct, next))(new PipelineRequest(request),  cancellationToken);
        }
    }
}
