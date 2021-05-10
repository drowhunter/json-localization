using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

namespace JsonLocalization.Localization
{
    public interface IJsonStringLocalizer : IBaseJsonStringLocalizer
    {
        new LocalizedString this[string name] { get; }
        new LocalizedString this[string name, params object[] arguments] { get; }
    }

    public class JsonStringLocalizer : BaseJsonStringLocalizer, IJsonStringLocalizer
    {

        public JsonStringLocalizer(ILocalizedJsonCache jsonCache, IHostEnvironment env) : base(jsonCache)
        {

        }

        public new LocalizedString this[string name]
        {
            get
            {
                var retval = base[name];
                return new LocalizedString(name, retval ?? string.Empty, retval == null, name);
            }
        }


        public new LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                string retval = base[name];

                return new LocalizedString(name, retval ?? string.Empty, retval == null, name);
            }
        }


    }
}