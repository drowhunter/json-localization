# JsonLocalization
Add the following to your `Startup.cs`
<details>
  <summary>View Code</summary>
  
```
    services.AddScoped<IJsonStringLocalizer, JsonStringLocalizer>();
    services.AddScoped<IJsonHtmlStringLocalizer, JsonHtmlStringLocalizer>();

    services.AddJsonLocalization(new[]
    {
        (cachename: Program.DASHBOARD_CLIENT_ALIAS, path: $@"{clientFolder}\{Environment.GetClientAlias()}\_locale" ),
        (cachename: Program.DASHBOARD_GLOBAL, path: $@"{clientFolder}\_locale")
    }, options =>
    {
        options.DefaultCulture = new CultureInfo("en");
        options.FallbackToDefaultLanguage = true;
        options.SupportedCultures = Configuration.GetList("SupportedCultures");
    });
            
```
</details>


