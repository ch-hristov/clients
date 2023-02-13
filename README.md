# d8n-dotnet-clients

Docs: https://documenter.getpostman.com/view/989374/2s935hS7qK

This is a library for .NET core 7. Please note that the .NET Framework version is still in development.

Nuget Package:
[![NuGet version (d8n-object-detection-core)](https://img.shields.io/nuget/v/d8n-object-detection-core.svg?style=flat-square)](https://www.nuget.org/packages/d8n-object-detection-core/)

# How to start

```csharp
using D8nObjectDetection;
using SixLabors.ImageSharp;

string api_key = Environment.GetEnvironmentVariable("d8n_api_key");

DocumentClient client = new DocumentClient(api_key);
var result = await client.RunAnalysis("./test.jpg");

var status = await client.GetStatus(result);
System.Console.WriteLine(status);
await Task.Delay(5000);
status = await client.GetStatus(result);
System.Console.WriteLine(status);

var objects = await client.GetCompleted(result);

System.Console.WriteLine(objects.Count());

foreach(var value in objects){
    Console.WriteLine(value.Class + " " + value.X1 + " " + value.Y1); // etc. etc..
}

```
Object now contains extracted info. For the full properties of what we can extract see this:
 https://github.com/ch-hristov/d8n-dotnet-clients/blob/b11294a0fc588b00be9b20baef886e9de530faf7/d8n-dotnet-client-core/DocumentClient.cs#L7

# Extract images

If you pass 1-st image to RunAnalysis(), you can get the second and third images
<div style="display:flex; flex-direction:row;">
<img src="./d8n-dotnet-example/test.jpg" width=200 height=200/>
<img src="./d8n-dotnet-example/result.png" width=200 height=200/>
<img src="./d8n-dotnet-example/line.png" width=200 height=200/>

Here's how:
 
`Note: `result` is the id variable returned from RunAnalysis()

# Limits
 1. Image size: 10MB
 2. Limits to number of requests apply.
