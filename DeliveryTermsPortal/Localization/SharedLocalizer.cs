using Newtonsoft.Json;
using System.Globalization;

namespace DeliveryTermsPortal.Localization
{
    public class SharedLocalizer : ISharedLocalizer
    {
        private readonly Dictionary<string, Dictionary<string, string>> _labels;

        public SharedLocalizer(IWebHostEnvironment env)
        {
            var path = Path.Combine(env.ContentRootPath, "Resources", "labels.json");
            _labels = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(path)) ?? new();

        }

        public string this[string key]
        {
            get
            {
                var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if(_labels.TryGetValue(key, out var texts))
                {
                    if (texts.TryGetValue(lang, out var text)) return text;
                    if (texts.TryGetValue("en", out var defaultText)) return defaultText;
                }
                return key;
            }
        }
    }
}
