# synonms Versioning #

Synonms Versioning uses attributes to associate versions with classes, properties and methods. Using this mechanism you can easily tailor APIs, data and associated documentation based on the requested version without having to provide duplicate code for each version.

Consider the following model that you are serialising and returning from a Web API as JSON:

```
public class MyResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    // This was added in v2.0 of the API
    public string BrandNewThingy { get; set; }
}
```

Usually, if you want to ensure the BrandNewThingy property is only available for calls to v2.0 of your API you would have two copies of your model - V1.MyResponse without the new property and V2.MyResponse with the new property. Then you have to make duplicates of your contollers as well so that V2 of your controller returns V2 of your model.  Faff and code duplication.

Using Synonms Versioning you simply version the properties using attributes and the library will prune the JSON for you.  This means you can serve multiple versions of the same API using a single copy of your controllers and models.

```
public class MyResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    [VersionHistory(IntroducedMajorVersion = 2, IntroducedMinorVersion = 0)]
    public string BrandNewThingy { get; set; }
}
```

You can apply the VersionHistoryAttribute to classes too:

```
[VersionHistory(IntroducedMajorVersion = 2, IntroducedMinorVersion = 0)]
public class MyFunkyNewData
{
    public int NoOfThings { get; set; }

    public string Description { get; set; }
}
```

##Synonms.Versioning.Core

Version tracking is handled by specifying Introduced and optionally Deprecated versions. The `VersionHistoryAttribute` class allows you to apply these values to classes or properties using simple annotations.

Extension methods enable you to interrogate decorated classes and properties to determine the `VersionHistory`. You can then supply the requested `Version` to the `IsApplicableAtVersion()` method to determine whether the class or property is relevant to that requested version.

```
var versionHistory = typeof(MyFunkyNewData).GetVersionHistory();
var requestedVersion = new Version(1, 0);
var isApplicable = versionHistory.IsApplicableAtVersion(requestedVersion);  // Returns false as the MyFunkyNewData class was only introduced in v2.0
```

This approach is used in the custom JSON serialisation, so that given a requested `Version`, objects or properties which are not relevant for that version can be pruned from the JSON.

##Synonms.Versioning.Web

The Web library adds support for ASP.NET Web API, with middleware to extract the requested `Version` from the HTTP request and inject it into the HttpContext.

Startup.cs:

```
public void ConfigureServices(IServiceCollection services)
{
    // You can add IApiVersionReaders in here using the factory thingy
    services.AddScoped<ApiVersionMiddleware>();
	
	// Inject the custom JSON serialiser into your controller
	services.AddScoped<IVersionableSerialiser, VersionableJsonSerialiser>();
}
```

Then in your Controller action:

```
var requestedVersion = HttpContext.GetRequestedVersion() ?? new Version();
var myResponse = //...Get your response model...
var json = _serialiser.Serialise(myResponse, requestedVersion);  // Where _serialiser is IVersionableSerialiser injected above

return Ok(json);

```

##Synonms.Versioning.Swashbuckle

The Swashbuckle library adds a document filter so that Swagger docs generated via Swashbuckle (Swashbuckle.AspNetCore.SwaggerGen) can be pruned.  To flag that a class should be included in version checking you need to apply the `VersionableSwaggerSchema` attribute:

```
[VersionHistory(IntroducedMajorVersion = 2, IntroducedMinorVersion = 0)]
[VersionableSwaggerSchema(typeof(MyFunkyNewData))]
public class MyFunkyNewData
{
    public int NoOfThings { get; set; }

    public string Description { get; set; }
}
```

You then need to add the filter in Startup.cs:

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c => 
	{
	    c.SwaggerDoc("v1", new OpenApiInfo
		{
		    Title = "My Awesome API",
			Description = "Do all the things",
			Version = "1.0"
		});
	    c.SwaggerDoc("v2", new OpenApiInfo
		{
		    Title = "My Even More Awesome API",
			Description = "Do all the things plus more",
			Version = "2.0"
		});
		
		c.EnableAnnotations();

        // Enable pruning of paths and schemas in Swagger docs
		c.DocumentFilter<VersionableDocumentFilter>();
	});
}
```

When generated, each version of the Swagger docs will only contain controller actions, models and properties applicable for that version.  Note that the "Version" of the SwaggerDoc must be compatible with the `System.Version(string version)` constructor otherwise the version can't be determined.
