using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace JsonLocalization.Localization
{
    public class JsonLocalizationOptions
    {
        public JsonLocalizationOptions()
        {
            DefaultCulture = new CultureInfo("en");
            SupportedCultures = new List<string>() {"en"};
        }
        /// <summary>
        /// The default culture ( defaults to en )
        /// </summary>
        public CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// If requested string is not found , controls whether the system will attempt to find the string in the default language
        /// </summary>
        public bool FallbackToDefaultLanguage { get; set; } = true;

        public List<string> SupportedCultures { get; set; }
    }


    public interface ILocalizedJsonCache
    {
        
        void Add(string name, string locale, JObject jobject);

        string Read(CultureInfo culture, string jsonpath, params object[] args);
    }

    public class LocalizedJsonCache : ILocalizedJsonCache
    {
        private readonly Dictionary<string, Dictionary<string, JObject>> _jdict;

        private readonly JsonLocalizationOptions _options;

        public LocalizedJsonCache()
        {
            _jdict = new Dictionary<string, Dictionary<string, JObject>>();
            _options = new JsonLocalizationOptions();
        }

        public LocalizedJsonCache(Action<JsonLocalizationOptions> optionsAction) : this()
        {
            _jdict = new Dictionary<string, Dictionary<string, JObject>>();

            optionsAction?.Invoke(_options);
        }

        public delegate void SetOptions(JsonLocalizationOptions options);

        public void Add(string name, string locale, JObject jobject)
        {
            if (_options.SupportedCultures.Contains(locale))
            {
                if (!_jdict.TryGetValue(name, out var jdict))
                {
                    jdict = new Dictionary<string, JObject>();
                    _jdict.Add(name, jdict);
                }

                if (!jdict.TryAdd(locale, jobject))
                {
                    throw new Exception($"Failed to add json resource to cache for language {locale}");
                }
            }
        }

        public string Read(CultureInfo culture, string jsonpath, params object[] args)
        {
            string retval = null;

            foreach (var dict in _jdict)
            {
                retval = Read(dict.Key, culture, jsonpath, args);
                if (retval != null)
                {
                    break;
                }
            }

            return retval;
        }

        public string Read(string name, CultureInfo culture, string jsonpath, params object[] args)
        {
            if (!_jdict.TryGetValue(name, out var jdict))
            {
                throw new Exception($"Cache does not exist for locale {name}");
            }

            string retval = null;

            if (jdict.TryGetValue(culture.Name, out var i18n))
            {
                try
                {
                    var selectToken = i18n.SelectToken(jsonpath);
                    retval = selectToken?.Value<string>();
                }
                catch (Exception )
                {
                    retval = "[" + jsonpath + "]";
                    // throw;
                }
            }

            if (retval == null && culture.Name != culture.TwoLetterISOLanguageName)
            {
                if (jdict.TryGetValue(culture.TwoLetterISOLanguageName, out i18n))
                {
                    retval = i18n.SelectToken(jsonpath)?.Value<string>();
                }
            }

            
            bool isDefault = culture.Name == _options.DefaultCulture.Name || culture.TwoLetterISOLanguageName == _options.DefaultCulture.TwoLetterISOLanguageName;

            if (retval == null  && !isDefault && _options.FallbackToDefaultLanguage)
            {
                if (jdict.TryGetValue(_options.DefaultCulture.Name, out i18n))
                {
                    retval = i18n.SelectToken(jsonpath)?.Value<string>();
                }

                if (retval == null && _options.DefaultCulture.Name != _options.DefaultCulture.TwoLetterISOLanguageName)
                {
                    if (jdict.TryGetValue(_options.DefaultCulture.TwoLetterISOLanguageName, out i18n))
                    {
                        retval = i18n.SelectToken(jsonpath)?.Value<string>();
                    }
                }
                //retval = jdict[_options.DefaultCulture.Name].SelectToken(jsonpath)?.Value<string>();


            }

            if (!string.IsNullOrWhiteSpace(retval) && args.Length > 0)
            {
                retval = string.Format(retval, args);
            }

            return retval;
        }

        
    }
}
