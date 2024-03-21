using Jint.Native.Object;
using Jzor.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CLR.Jzor
{
    /// <summary>
    /// Demonstration class
    /// </summary>

    [TsDoc("HTTP client instance")]
    public class HttpClient
    {
        public class Initializer : PluginInitializer
        {
            public override void Build(WebApplicationBuilder builder)
            {
                builder.Services.AddHttpClient();
            }

            public override void Register(IPluginRegistry registry)
            {
                registry.RegisterType<Uri>();
                registry.RegisterType<Request>();
                registry.RegisterType<Response>();
                registry.RegisterType<HttpResponseMessage>(); // Types used by the response message also needs to be registered to provide fully typed access
                registry.RegisterSingleton<HttpClient>();
            }
        }

        private global::System.Net.Http.HttpClient _client;
        private IScriptEngine _scriptEngine;

        public HttpClient(IPluginManager pluginManager)
        {
            _scriptEngine = pluginManager.ScriptEngine;
            _client = pluginManager.GetService<IHttpClientFactory>().CreateClient();
        }

        [TsDoc("Sends a HTTP request and returns the response as a CLR object")]
        public async Task<Response> SendAsync(string method, string uri)
            => await SendAsync(new Request(method, uri));

        [TsDoc("Sends a HTTP request and returns the response as a CLR object")]
        public async Task<Response> SendAsync(Request request)
        {
            var method = new HttpMethod(request.Method);
            var uri = new Uri(request.Uri);
            var req = new HttpRequestMessage(method, uri);
            var resp = await _client.SendAsync(req);

            return new Response
            {
                Content = await resp.Content.ReadAsStringAsync(),
                Headers = resp.Headers.ToString(),
                Status = resp.StatusCode.ToString(),
                StatusCode = (int)resp.StatusCode,
            };
        }

        [TsDoc("Sends HTTP request, using a JavaScript request object and also returns the response as a JavaScript object")]
        public async Task<ObjectInstance> SendAsync(ObjectInstance request)
        {
            var method = new HttpMethod((await request.GetAsync("method")).ToString());
            var uri = new Uri((await request.GetAsync("uri")).ToString());
            var req = new HttpRequestMessage(method, uri);
            var resp = await _client.SendAsync(req);

            var result = _scriptEngine.CreateJsObject();
            result.Set("statuscode", (int)resp.StatusCode);
            result.Set("status", resp.StatusCode.ToString());
            result.Set("headers", resp.Headers.ToString());
            result.Set("content", await resp.Content.ReadAsStringAsync());
            return result;
        }

        
        [TsDoc("Sends a HTTP request, using a JAvaScript reqiest object and returns a full CLR HttpResponseMessage")]
        public Task<HttpResponseMessage> SendAsync2(ObjectInstance request)
        {
            var method = new HttpMethod(request.Get("method").ToString());
            var uri = new Uri(request.Get("uri").ToString());
            var req = new HttpRequestMessage(method, uri);
            return _client.SendAsync(req);
        }
    }

    [TsDoc("HTTP request")]
    public class Request
    {
        public string Method;
        public string Uri;

        public Request(string method, string uri)
        {
            Method = method;
            Uri = uri;
        }
    }

    [TsDoc("HTTP response")]
    public class Response
    {
        public string Content { get; set; }
        public string Headers { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
    }
}