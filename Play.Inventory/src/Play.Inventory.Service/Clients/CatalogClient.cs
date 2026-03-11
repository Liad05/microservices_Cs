using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient _httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDTO>> GetCatalogItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDTO>>("/items");
        }
    }
}
