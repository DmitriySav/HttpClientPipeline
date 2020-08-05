# HttpClientPipeline
Http client interseptor as .net core pipeline 


Interceptor example:

    public class AuthInterceptor : IRequestMiddleware
    {
        private AuthStateProvider _authStateProvider;

        public AuthInterceptor(
            AuthStateProvider authStateProvider
            )
        {
            _authStateProvider = authStateProvider;
        }

        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request, CancellationToken cancellationToken, IRequestMiddleware.RequestHandlerDelegate next)
        {
            var needToRefresh = await _authStateProvider.IsAuthenticatedAsync();

            var response = await next(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _authStateProvider.RefreshTokenAsync();

                response = await next(request, cancellationToken);
            }

            return response;
        }



Registration example: 

            builder.Services.AddScoped(sp => new HttpPipelineClient(sp, sp.GetService<HttpPipeline>())
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            builder.Services.AddScoped<HttpPipeline>();
            builder.Services.AddScoped<IRequestMiddleware, AuthInterceptor>();
