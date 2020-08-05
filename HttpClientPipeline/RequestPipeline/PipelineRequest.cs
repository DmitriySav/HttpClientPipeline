using System.Net.Http;

namespace HttpClientPipeline.RequestPipeline
{
    public class PipelineRequest : HttpRequestMessage
    {
        public PipelineRequest(HttpRequestMessage request):base()
        {
            Content = request.Content;
            foreach (var httpRequestHeader in request.Headers)
            {
                Headers.Add(httpRequestHeader.Key, httpRequestHeader.Value);
            }

            foreach (var requestProperty in request.Properties)
            {
                Properties.Add(requestProperty.Key, requestProperty.Value);
            }

            Method = request.Method;
            Version = request.Version;
            RequestUri = request.RequestUri;
        }
    }
}
