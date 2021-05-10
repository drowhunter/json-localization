using System.Globalization;

namespace JsonLocalization.Localization
{
    public interface IBaseJsonStringLocalizer
    {
        string this[string name] { get; }
        string this[string name, params object[] arguments] { get; }
    }

    public abstract class BaseJsonStringLocalizer : IBaseJsonStringLocalizer
    {
        private readonly ILocalizedJsonCache _jsonCache;
        
        protected BaseJsonStringLocalizer(ILocalizedJsonCache jsonCache)
        {
            _jsonCache = jsonCache;
            //this.CacheName = cachename;
        }

        //protected string CacheName { get; set; }

        public virtual string this[string name]
        {
            get
            {
                return _jsonCache.Read(CultureInfo.CurrentUICulture, name);
            }
        }

        public virtual string this[string name, params object[] arguments]
        {
            get
            {
                return _jsonCache.Read(CultureInfo.CurrentUICulture, name, arguments);
            }
        }


    }
}