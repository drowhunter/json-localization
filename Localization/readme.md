# Json Localization


## About
Use this library to load i18n compliant json files instead of using `resx`


## Usage
The way this library work is you create a json file named `strings.{culturecode}.json` in a folder of your choosing

Then in your startup you register your i18n json files like this

```
services.AddJsonLocalization(new (string cachename, string path)[]
{
    (cachename: Program.DASHBOARD_TENANT_ALIAS, path: "Tenants\\" + Environment.GetTenantAlias()),
    (cachename: Program.DASHBOARD_GLOBAL, path: "Tenants")
}, options =>
{
    options.DefaultCulture = new CultureInfo("en");
    options.FallbackToDefaultLanguage = true;
});

```
**cachename** = the unique name of the local cache
**path** = the path to the folder you strings.json files are in

**options.DefaultCulture** = the default culture to fall back to should the resource not be found in the requested culture *(defaults to "en')*

**options.FallbackToDefaultLanguage** = if the resource is not found in the requested culture, enables falling back to the default culture specified in `options.DefaultCulture` *(defaults to true)*

## Fallback flow

When the requested resource is not found the system fallsback as follows:

### Example
Request `fr-CA`
- if not found try `fr`
- if not found and `options.FallbackToDefaultLanguage = true`
    * search culture set on `options.DefaultCulture`


