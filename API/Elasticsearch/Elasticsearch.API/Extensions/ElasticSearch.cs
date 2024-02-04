using Elasticsearch.Net;
using Nest;

namespace Elasticsearch.API.Extensions
{
    public static class ElasticSearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elasticsearch")["Url"]!));
            var connectionSettings = new ConnectionSettings(pool);
            var client = new ElasticClient(connectionSettings);
            services.AddSingleton(client);
        }
    }
}
