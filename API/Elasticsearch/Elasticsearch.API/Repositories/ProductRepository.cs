using Elasticsearch.API.Dtos;
using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string indexName = "products44";
        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }
        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            // Id yi boş gönderiyoruz,  ES buradaki tanımımıza göre id üretiyor. 
            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

            // fast fail
            if (!response.IsValid) return null;

            newProduct.Id = response.Id;

            return newProduct;
        }
        public async Task<IReadOnlyCollection<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return result.Documents;
        }
        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));

            if (!response.IsValid) return null;

            response.Source.Id = id;

            return response.Source;
        }
        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(updateProduct.Id, x => x.Index(indexName).Doc(updateProduct));

            return response.IsValid; 
        }
        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));

            return response;
        }
    }
}
