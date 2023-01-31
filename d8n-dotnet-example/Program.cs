using D8nObjectDetection;
using SixLabors.ImageSharp;

string api_key = Environment.GetEnvironmentVariable("d8n_api_key");

DocumentClient client = new DocumentClient(api_key);
var result = await client.RunAnalysis("./test.jpg");

await Task.Delay(3000);
var objects = await client.GetCompleted(result);

System.Console.WriteLine(objects.Count());

var symbolImage = await client.GetSymbols(result);
symbolImage.Save("./result.png");

var lineImage = await client.GetLines(result);
System.Console.WriteLine(lineImage);
lineImage.Save("./line.png");