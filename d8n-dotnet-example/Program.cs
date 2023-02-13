using D8nObjectDetection;
using SixLabors.ImageSharp;

string api_key = Environment.GetEnvironmentVariable("api_key");

if(api_key == null) throw new Exception("Failed to load API key");

DocumentClient client = new DocumentClient(api_key);
var result = await client.RunAnalysis("./test.jpg");

var status = await client.GetStatus(result);
System.Console.WriteLine(status);
await Task.Delay(5000);
status = await client.GetStatus(result);
System.Console.WriteLine(status);

var objects = await client.GetCompleted(result);

System.Console.WriteLine(objects.Count());

var symbolImage = await client.GetSymbols(result);
symbolImage.Save("./result.png");

var lineImage = await client.GetLines(result);
lineImage.Save("./line.png");