using Jint;
using Jint.Native;
using System.Globalization;
using System.Text.RegularExpressions;
using Jzor.Plugins;

namespace CLR.Jzor
{
    [TsDoc("String plugin")]
    public class String
    {
        public class Initializer : PluginInitializer
        {
            public override void Register(IPluginRegistry registry)
            {
                registry.RegisterSingleton<String>();
            }
        }

        private readonly Regex sortRegex = new(@"\d+", RegexOptions.Compiled);
        private IScriptEngine _scriptEngine;

        public String(IPluginManager pluginManager)
        {
            _scriptEngine = pluginManager.ScriptEngine;
        }

        //private CultureInfo _culture => _engine.CurrentCulture;

        [TsDoc("Compares alphanumeric strings by left padding numbers with zero up to a total width of 10 digits")]
        public int AlphaNumericComparer(string a, string b)
        {
            var valueA = sortRegex.Replace(a, match => match.Value.PadLeft(10, '0'));
            var valueB = sortRegex.Replace(b, match => match.Value.PadLeft(10, '0'));
            return string.Compare(valueA, valueB, _scriptEngine.CurrentCulture, CompareOptions.Ordinal);
        }

        [TsDoc("Provides C# (.net) string format capabilities")]
        public string Format(string format, params object[] args) => string.Format(_scriptEngine.CurrentCulture, format, args);

        [TsDoc("Formats a JavaScript string using the C# (.net) string format function")]
        public JsValue Format(JsValue format, params JsValue[] args)
        {
            var formatString = args.ElementAtOrDefault(0)?.ToString();

            var obj = format.ToObject();
            if (format.IsBoolean()) return ((bool)obj).ToString(_scriptEngine.CurrentCulture);
            if (format.IsDate()) return ((DateTime)obj).ToString(_scriptEngine.CurrentCulture);
            if (format.IsNumber()) return ((double)obj).ToString(_scriptEngine.CurrentCulture);

            // TODO: Add default ClrTypes
            return format.ToString();
        }
    }
}