

using Play.Invetory.Service.Dtos;

namespace Play.Invetory.Service.Clients
{
    public class CatalogClient 
    {
        private readonly HttpClient httpClient;
        public CatalogClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IReadOnlyCollection<CatalogItemDto>> GetAllAsync()
        {
            return await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/api/items") ?? throw new NullReferenceException();
        }
    }
}
