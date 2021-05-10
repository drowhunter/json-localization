using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLocalization.Localization
{

    public static class JsonLocalizationExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, (string cachename, string path)[] items, Action<JsonLocalizationOptions> optionsAction = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (optionsAction == null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }

            IServiceCollection addJsonLocalization = null;

            services.AddScoped<IJsonStringLocalizer, JsonStringLocalizer>();
            services.AddScoped<IJsonHtmlStringLocalizer, JsonHtmlStringLocalizer>();

            try
            {
                addJsonLocalization = services.AddSingleton<ILocalizedJsonCache, LocalizedJsonCache>(provider =>
                    {
                        var cache = new LocalizedJsonCache(optionsAction);

                        foreach ((string cachename, string path) in items)
                        {
                            string folder = Path.Combine(Directory.GetCurrentDirectory(), path);
                            var strings = Directory.GetFiles(folder, "*.json").Select(f => new FileInfo(f));


                            foreach (var file in strings)
                            {
                                var match = Regex.Match(file.Name, @"^(?<lang>[^\.]+)\.json$");
                                if (match.Success)
                                {
                                    string lang = match.Groups["lang"].Value;
                                    cache.Add(cachename, lang,
                                        JObject.Load(new JsonTextReader(file.OpenText())));
                                }
                                else
                                {
                                    throw new Exception("Could not figure out locale of file " + file.Name);
                                }
                            }
                        }

                        return cache;
                    });
            }
            catch (Exception x)
            {

                
            }

            return addJsonLocalization;

            
        }
    }
}
