using D8nObjectDetection;

DocumentClient client = new DocumentClient("");
var result = await client.RunAnalysis("./test.jpg");

await Task.Delay(3000);
var objects = await client.GetCompleted(result);
