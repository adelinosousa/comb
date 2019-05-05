[![Build Status](https://dev.azure.com/lino-playroom/Comb/_apis/build/status/adelinosousa.comb?branchName=master)](https://dev.azure.com/lino-playroom/Comb/_build/latest?definitionId=1&branchName=master)

# Comb
Simple C# web crawler. Discover accessible website resources and sitemap. Supports `.net framework`, `.net standard` and `.net core`.

## Demo
Coming soon!!!

## How to use

### Setup

Use package-manager to install
```csharp
Install-Package Site.Comb
```
In your start up class, during the service configuration, invoke `services.AddSiteComb();`. This will setup the required dependencies this tool needs. Example:
```csharp
using Site.Comb;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSiteComb();
    }
}
```

### Usage

Resolve the `IComb` dependency and invoke `.Brush()` with `CombRequest`. Example:
```csharp
[Route("api/[controller]")]
[ApiController]
public class SampleController : ControllerBase
{
    private readonly IComb comb;

    public SampleController(IComb comb)
    {
        this.comb = comb;
    }

    public async Task<CombResponse> Get(string url)
    {
        var response = await comb.Brush(new CombRequest
        {
            Url = url
        });

        return response;
    }
}
```

### Request options

| Property | Description |
|----------|-------------|
| Url      | Target url to inspect and comb resources for |
| Depth*   | Depth level, how **deep** to comb for. Default is set to 1 |

**NOTE*** the bigger the interger, the longer it will take for the process to complete.

### Response querying

To access the first level of descendants like this:
```csharp
var descendants = response.Result.Descendants;
foreach (var descendant in descendants)
{
    var descendantUrl = descendant.Value;
    var children = descendant.Descendants;
}
```

You can filter all **links** found by their type. Example:
```csharp
var images = response.Result.All(CombLinkType.IMG);
```
This will return all images found within the given target url. Supported types are `.png`, `.jpg`, `.jpeg`, `.tif`, `.tiff`, `.bmp` and `.gif`. You can filter by group type, such as `IMG`, or individually.

## Suggestions
Contact me if you have any suggestions or improvements.

## License
[LICENSE](LICENSE.md)
