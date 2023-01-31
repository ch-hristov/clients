# d8n-dotnet-clients

Docs: https://documenter.getpostman.com/view/989374/2s935hS7qK

Nuget Package:
https://www.nuget.org/packages/d8n-object-detection-core/

Run engineering drawing analysis:

```
using D8nObjectDetection;

DocumentClient client = new DocumentClient("");
var result = await client.RunAnalysis("./test.jpg");

await Task.Delay(3000);
var objects = await client.GetCompleted(result);
```

You can query for status:

```
var objects = await client.GetStatus(result);
```
