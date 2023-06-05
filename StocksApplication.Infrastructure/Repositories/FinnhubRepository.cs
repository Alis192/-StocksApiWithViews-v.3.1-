using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net;
using StocksApplication.Core.Domain.RepositoryContracts;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration= configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            string? token = _configuration["userToken"];

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={token}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                
                string response = streamReader.ReadToEnd();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDictionary == null )
                {
                    throw new InvalidOperationException("No Response from Finnhub service");
                }

                if (responseDictionary.ContainsKey("error")) 
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

                return responseDictionary;
            }
        }


        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            string? token = _configuration["userToken"];

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={token}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                
                string responseAsString = streamReader.ReadToEnd();

                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseAsString);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No Response from Finnhub service");
                }

                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

                return responseDictionary;

            }
        }


        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            string? token = _configuration["userToken"];

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream(); 
                StreamReader streamReader = new StreamReader(stream);
                string responseAsString = streamReader.ReadToEnd();

                List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseAsString);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No Response from Finnhub service");
                }

                return responseDictionary;

            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            string? token = _configuration["userToken"];

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                string responseAsString = streamReader.ReadToEnd();

                Dictionary<string, object> responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseAsString);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No Response from Finnhub service");
                }

                return responseDictionary;
            }
        }
    }
}