# JsonLocalization
## Overview

All you have to do is add named json dictionaries in order and point to the folder location

---
**Consider the folling folder structure.**

* i18n\
  * _locale\
    - en.json
    - en-GB.json
    - fr.json
* client1\
  * _locale\
     - en.json
     - fr.json
* client2\
  * _locale\
    - en.json
    - fr.json
--- 

Add the following to your `Startup.cs`

  
```
    services.AddJsonLocalization(new[]
    {
        (cachename: "client", path: $@"i18n\{client}\_locale" ),
        (cachename: "global", path: $@"i18n\_locale")
    }, options =>
    {
        options.DefaultCulture = new CultureInfo("en");
        options.FallbackToDefaultLanguage = true;
        options.SupportedCultures = Configuration.GetList("SupportedCultures");
    });
            
```

## Options
<table>
<thead>
    <tr>
        <th>Property</th>
        <th>Description</th>
        <th>Default</th>
    </tr>
</thead>
<tbody>
    <tr>
    <td>DefaultCulture</td><td>Default culture to use if none specified</td>
    <td>en</td>
    <tr>
    <td>FallbackToDefaultLanguage</td><td>If true if a string is not found in the current culture, it will fallback to the default culture. **Note** that if region code is specified (e.g. "en-GB") it will be searched first.</td>
    <td>true</td>
    <tr>
    <td>SupportedCultures</td><td>Default culture to use if none specified</td>
    <td>["en"]</td>
</tbody>
</table>
