using Jint.Native;
using Jzor.Configuration;
using Jzor.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;

namespace CLR.MudBlazor
{
    public class PluginInitializer : IPluginInitializer
    {
        private static bool _enabled = false;

        public void Build(WebApplicationBuilder builder)
        {
            var config = builder.Configuration.GetSection("MudBlazorPlugin")?.Get<MudBlazorConfig>();
            _enabled = config?.Enabled != false;
            if (_enabled) builder.Services.AddMudServices();
            ConfigHelper.ConfigLogger.LogInformation($"MudBlazorPlugin (enabled:{_enabled})");
        }

        public void Configure(IApplicationBuilder app)
        {
            if (!_enabled) return;
        }

        public void Register(IPluginRegistry registry)
        {
            // Testing a few MudBlazor components
            registry.RegisterType<JsValue>();
            registry.RegisterType<MudAlert>();
            registry.RegisterType<MudInput<JsValue>>();
            registry.RegisterType<MudBaseInput<JsValue>>();

            registry.RegisterType<SnackbarConfiguration>();
            registry.RegisterSingleton<ISnackbar, SnackbarService>();
            registry.RegisterType<SnackbarOptions>();
            registry.RegisterType<MudSnackbarProvider>();

            registry.RegisterType<MudThemeProvider>();
            registry.RegisterType<MudDialogProvider>();
            registry.RegisterType<MudDialog>();
            registry.RegisterSingleton<IDialogService, DialogService>();

            registry.RegisterType<HorizontalAlignment>();
            registry.RegisterType<AlertTextPosition>();
            registry.RegisterType<MouseEventArgs>();
            registry.RegisterType<Severity>();
            registry.RegisterType<Variant>();
            registry.RegisterType<Color>();
        }
    }
}