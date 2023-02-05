using System;
using System.Text.Json;
using Newtonsoft.Json;
using RestSharp;
using SixLabors.ImageSharp;

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
        throw new Exception($"We couldn't fetch the data for you : {response.StatusCode} {response.Content}");
    }

    /// <summary>
    /// Fetch symbol results for a request id
    /// </summary>
    /// <param name="id">The processing ID to use</param>
    /// <returns>An image of the identified symbols for the added id</returns>
    /// <exception cref="KeyNotFoundException">Couldn't find data for this ID</exception>
    /// <exception cref="NullReferenceException">The response was empty or wasn't as expected. The service might be down. Check status</exception>
    /// <exception cref="UnauthorizedAccessException">Check your API key</exception>
    /// <exception cref="Exception">Something went wrong.</exception>
    public async Task<Image> GetSymbols(string id)
    {
        var client = new RestClient($"http://159.223.120.61:5000/api/get_symbols?id={id}");
        var request = new RestRequest();

        request.AddHeader("API-KEY", this._apiKey);
        request.Method = Method.Get;
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = response.Content;
            if (content == null) throw new NullReferenceException("Something went wrong fetching results for analysis");

            var image = Image.Load(response.RawBytes);
            return image;
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
    /// Fetch symbol results for a request id
    /// </summary>
    /// <param name="id">The processing ID to use</param>
    /// <returns>An image of the identified lines for the added id</returns>
    /// <exception cref="KeyNotFoundException">Couldn't find data for this ID</exception>
    /// <exception cref="NullReferenceException">The response was empty or wasn't as expected. The service might be down. Check status</exception>
    /// <exception cref="UnauthorizedAccessException">Check your API key</exception>
    /// <exception cref="Exception">Something went wrong.</exception>
    public async Task<Image> GetLines(string id)
    {
        var client = new RestClient($"https://d8n.xyz/api/get_lines?id={id}");
        var request = new RestRequest();

        request.AddHeader("API-KEY", this._apiKey);
        request.Method = Method.Get;
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = response.Content;
            if (content == null) throw new NullReferenceException("Something went wrong fetching results for analysis");

            var image = Image.Load(response.RawBytes);
            return image;
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
    /// Use this to determine if a request is ready.
    /// Fetches status for a request id. Returns a string in the form Status.Started|Status.Symbol_Extraction etc. etc.
    /// Once a request is ready the status bcomes Status.Completed.
    /// </summary>
    /// <param name="id">The processing ID to use</param>
    /// <returns>Status of the request</returns>
    /// <exception cref="NullReferenceException">Something went wrong with the service</exception>
    /// <exception cref="KeyNotFoundException">Couldn't find this request</exception>
    /// <exception cref="UnauthorizedAccessException">Check your API key</exception>
    /// <exception cref="Exception">Something went wrong with the service</exception>
    public async Task<string> GetStatus(string id)
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
