using Jzor.Plugins;
using Markdig;

namespace CLR.Jzor
{
    [TsDoc("Markdown plugin instance for converting markdown into HTML")]
    public class Markdown
    {
        public class Initializer : PluginInitializer
        {
            public override void Register(IPluginRegistry registry)
            {
                registry.RegisterSingleton<Markdown>();
            }
        }

        private MarkdownPipeline _pipeline;

        public Markdown(IPluginManager pluginManager)
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        [TsDoc("Renders the markdown to HTML")]
        public string ToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown, _pipeline);
        }
    }
}