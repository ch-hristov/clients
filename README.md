# d8n-dotnet-clients

Docs: https://documenter.getpostman.com/view/989374/2s935hS7qK

Nuget Package:
[![NuGet version (d8n-object-detection-core)](https://img.shields.io/nuget/v/d8n-object-detection-core.svg?style=flat-square)](https://www.nuget.org/packages/d8n-object-detection-core/)

Run engineering drawing analysis:

```
using D8nObjectDetection;

DocumentClient client = new DocumentClient("");
var result = await client.RunAnalysis("./test.jpg");

// Query for status once in a while (until status is equal to "Completed")
var objects = await client.GetStatus(result);

await Task.Delay(3000);
// Returns the object coordinates. See
var objects = await client.GetCompleted(result);
```
Object now contains extracted info. For the full properties of what we can extract see this:
 https://github.com/ch-hristov/d8n-dotnet-clients/blob/b11294a0fc588b00be9b20baef886e9de530faf7/d8n-dotnet-client-core/DocumentClient.cs#L7)


