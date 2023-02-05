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

var symbolImage = await client.GetSymbols(result);
symbolImage.Save("./result.png");

var lineImage = await client.GetLines(result);
lineImage.Save("./line.png");