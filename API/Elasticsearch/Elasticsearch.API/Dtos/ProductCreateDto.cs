using Elasticsearch.API.Models;

namespace Elasticsearch.API.Dtos
{
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto FeatureDto)
    {
        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature
                {
                    Width = FeatureDto.Width,
                    Height = FeatureDto.Height,
                    Color = (Color)int.Parse(FeatureDto.Color)
                }
            };

        }

    }
}
