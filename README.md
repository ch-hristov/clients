# d8n-dotnet-clients


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
