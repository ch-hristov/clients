using System;
using System.Text.Json;
using Newtonsoft.Json;
using RestSharp;

namespace D8nObjectDetection;
public class DrawingObject
{
    [JsonProperty("_class")]
    public double Class { get; set; }
    [JsonProperty("x1")]
    public double X1 { get; set; }

    [JsonProperty("y1")]
    public double Y1 { get; set; }

    [JsonProperty("x2")]
    public double X2 { get; set; }

    [JsonProperty("y2")]
    public double Y2 { get; set; }

    [JsonProperty("confidence")]
    public double Confidence { get; set; }

    // Lien or Text
    [JsonProperty("type")]
    public string Type { get; set; } = "";

    [JsonProperty("segment")]
    public List<int> Segments { get; set; } = new List<int>();

    [JsonProperty("is_edge_node")]
    public bool IsEdge { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; } = "";
}
public class DocumentClient
{
    private string _apiKey = "";
    public DocumentClient(string apiKey)
    {
        this._apiKey = apiKey;
    }

    public async Task<string?> RunAnalysis(string filePath)
    {
        var client = new RestClient("https://d8n.xyz/api/analysis");
        var request = new RestRequest();


        request.AddHeader("API-KEY", this._apiKey);
        request.Method = Method.Post;
        request.AddFile("file", filePath);

        var response = await client.ExecuteAsync(request);
        System.Console.WriteLine(response.Content);
        System.Console.WriteLine(response.StatusCode);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = response.Content;
            if (content == null) throw new NullReferenceException("Something went wrong fetching results for analysis");

            dynamic result = JsonConvert.DeserializeObject(content);
            return result["id"];
        }
        throw new Exception("Failed to fetch result!");
    }

    public async Task<IEnumerable<DrawingObject>> GetCompleted(string id)
    {
        var client = new RestClient($"https://d8n.xyz/api/completed?id={id}");
        var request = new RestRequest();

        request.AddHeader("API-KEY", this._apiKey);
        request.Method = Method.Get;

        var response = await client.ExecuteAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = response.Content;
            if (content == null) throw new NullReferenceException("Something went wrong fetching results for analysis");
            System.Console.WriteLine(content);
            var result = JsonConvert.DeserializeObject<IEnumerable<DrawingObject>>(content);
            return result;
        }
        throw new Exception("Failed to fetch result!");
    }

    public async Task<string?> GetStatus(string id)
    {
        var client = new RestClient($"https://d8n.xyz/api/get_status?id={id}");
        var request = new RestRequest();

        request.AddHeader("API-KEY", this._apiKey);
        request.Method = Method.Get;

        var response = await client.ExecuteAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = response.Content;
            if (content == null) throw new NullReferenceException("Something went wrong fetching results for analysis");
            dynamic result = JsonConvert.DeserializeObject(content);
            return result["status"];
        }
        throw new Exception("Failed to fetch result!");
    }


}
