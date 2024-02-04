namespace Elasticsearch.API.Dtos
{
    public record ProductUpdateDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto FeatureDto)
    {
    }
}
