using Elasticsearch.Net;
using ElasticSearchPOC.Models;
using System.Text.Json;

namespace ElasticSearchPOC.Services;

public class ElasticSearchService
{
    private readonly ElasticLowLevelClient _client;
    private readonly string _indexName = "products";

    public ElasticSearchService(string connectionString = "http://localhost:9200")
    {
        var settings = new ConnectionConfiguration(new Uri(connectionString));
        _client = new ElasticLowLevelClient(settings);
    }

    public async Task<bool> IndexProductAsync(Product product)
    {
        var response = await _client.IndexAsync<StringResponse>(_indexName, product.Id, 
            PostData.Serializable(product));
        return response.Success;
    }

    public async Task<Product?> GetProductAsync(string id)
    {
        var response = await _client.GetAsync<StringResponse>(_indexName, id);
        
        if (!response.Success) return null;
        
        var jsonDoc = JsonDocument.Parse(response.Body);
        var source = jsonDoc.RootElement.GetProperty("_source");
        
        return JsonSerializer.Deserialize<Product>(source.GetRawText());
    }

    public async Task<List<Product>> SearchProductsAsync(string query)
    {
        var searchQuery = new
        {
            query = new
            {
                multi_match = new
                {
                    query = query,
                    fields = new[] { "name", "description", "category" }
                }
            }
        };

        var response = await _client.SearchAsync<StringResponse>(_indexName, 
            PostData.Serializable(searchQuery));

        if (!response.Success) return new List<Product>();

        var jsonDoc = JsonDocument.Parse(response.Body);
        var hits = jsonDoc.RootElement.GetProperty("hits").GetProperty("hits");
        
        var products = new List<Product>();
        foreach (var hit in hits.EnumerateArray())
        {
            var source = hit.GetProperty("_source");
            var product = JsonSerializer.Deserialize<Product>(source.GetRawText());
            if (product != null) products.Add(product);
        }
        
        return products;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var response = await _client.DeleteAsync<StringResponse>(_indexName, id);
        return response.Success;
    }
}
