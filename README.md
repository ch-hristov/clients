# d8n-dotnet-clients

Docs: https://documenter.getpostman.com/view/989374/2s935hS7qK

Nuget Package:
[![NuGet version (d8n-object-detection-core)](https://img.shields.io/nuget/v/d8n-object-detection-core.svg?style=flat-square)](https://www.nuget.org/packages/d8n-object-detection-core/)

# Basics

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
 https://github.com/ch-hristov/d8n-dotnet-clients/blob/b11294a0fc588b00be9b20baef886e9de530faf7/d8n-dotnet-client-core/DocumentClient.cs#L7

# Extract images

## Here's an example:

<div style="display:flex; flex-direction:row;">
<img src="./d8n-dotnet-example/test.jpg" width=200 height=200/>
<img src="./d8n-dotnet-example/result.png" width=200 height=200/>
<img src="./d8n-dotnet-example/line.png" width=200 height=200/>

 ## Here's how:
 
```
var symbolImage = await client.GetSymbols(result);
symbolImage.Save("./data.png");
 
var lineImage = await client.GetLines(result);
symbolImage.Save("./line.png");
 
```
