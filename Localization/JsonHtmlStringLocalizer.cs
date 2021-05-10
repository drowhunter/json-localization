using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Hosting;

namespace JsonLocalization.Localization
{
    public interface IJsonHtmlStringLocalizer : IBaseJsonStringLocalizer
    {
        new LocalizedHtmlString this[string name] { get; }
        new LocalizedHtmlString this[string name, params object[] arguments] { get; }
    }
    public class JsonHtmlStringLocalizer : BaseJsonStringLocalizer, IJsonHtmlStringLocalizer
    {

        public JsonHtmlStringLocalizer(ILocalizedJsonCache jsonCache, IHostEnvironment env) : base(jsonCache)
        {

        }

        public new LocalizedHtmlString this[string name]
        {
            get
            {
                var retval = base[name];
                return new LocalizedHtmlString(name, retval ?? string.Empty, retval == null, name);
            }
        }


        public new LocalizedHtmlString this[string name, params object[] arguments]
        {
            get
            {
                string retval = base[name, arguments];

                return new LocalizedHtmlString(name, retval ?? string.Empty, retval == null, name);
            }
        }


    }
}