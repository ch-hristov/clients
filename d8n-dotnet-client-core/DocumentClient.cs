using System;
using System.Text.Json;
using Newtonsoft.Json;
using RestSharp;

namespace D8nObjectDetection;


public enum Type
{
    [JsonProperty("line")]
    Line,
    [JsonProperty("symbol")]
    Symbol,
    [JsonProperty("text")]
    Text
}

public enum Status
{
    _None = 1,
    Started = 2,
    Symbol_Detection = 4,
    Line_Detection = 8,
    Text_Extraction = 16,
    Rejected = 32,
    Failed = 64,
    Completed = 128
}

/// <summary>
/// Represents an object identified in the drawing.
/// </summary>
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

    [JsonProperty("type")]
    public Type Type { get; set; }

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

    /// <summary>
    /// Execute analysis for the drawing
    /// </summary>
    /// <param name="filePath">The path to the resulting request id</param>
    /// <returns>ID for the drawing processing</returns>
    /// <exception cref="NullReferenceException">The response was empty or wasn't as expected. The service might be down. Check status</exception>
    /// <exception cref="UnauthorizedAccessException">Check your API key</exception>
    /// <exception cref="Exception">Something went wrong.</exception>
    public async Task<string?> RunAnalysis(string filePath)
    {
        var client = new RestClient("https://d8n.xyz/api/analysis");
        var request = new RestRequest();

        request.AddHeader("API-KEY", this._apiKey);
        request.Method = Method.Post;
        request.AddFile("file", filePath);

        var response = await client.ExecuteAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = response.Content;
            if (content == null) throw new NullReferenceException("Something went wrong fetching results for analysis");

            dynamic result = JsonConvert.DeserializeObject(content);
            return result["id"];
        }
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("An error occurred. Check your API keys and the ID of the request.");
        }
        throw new Exception("We couldn't fetch the data for you");
    }

    /// <summary>
    /// Fetch completed results for a request id
    /// </summary>
    /// <param name="id">The processing ID to use</param>
    /// <returns>An list of drawing object identified for the added id</returns>
    /// <exception cref="KeyNotFoundException">Couldn't find data for this ID</exception>
    /// <exception cref="NullReferenceException">The response was empty or wasn't as expected. The service might be down. Check status</exception>
    /// <exception cref="UnauthorizedAccessException">Check your API key</exception>
    /// <exception cref="Exception">Something went wrong.</exception>
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
            var result = JsonConvert.DeserializeObject<IEnumerable<DrawingObject>>(content);
            return result;
        }
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException();
        }
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("An error occurred. Check your API keys and the ID of the request.");
        }
        throw new Exception("We couldn't fetch the data for you");
    }

    /// <summary>
    /// Fetch status for a request id
    /// </summary>
    /// <param name="id">The processing ID to use</param>
    /// <returns>Status of the request</returns>
    /// <exception cref="NullReferenceException">Something went wrong with the service</exception>
    /// <exception cref="KeyNotFoundException">Couldn't find this request</exception>
    /// <exception cref="UnauthorizedAccessException">Check your API key</exception>
    /// <exception cref="Exception">Something went wrong with the service</exception>
    public async Task<Status> GetStatus(string id)
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
            return Enum.Parse<Status>(result["status"]);
        }
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException();
        }
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("An error occurred. Check your API keys and the ID of the request.");
        }
        throw new Exception("We couldn't fetch the data for you");
    }


}
