using OkooraAssignment.Models.FxRateModels;
using System.Text.Json;

namespace OkooraAssignment.Services
{
    public class RateFetcherService: IHostedService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private Timer? _timer;

        public RateFetcherService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        #region Timer handling

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartRateFetching();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0); // Stop the timer when the application shuts down
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void StartRateFetching()
        {
            _timer = new Timer(async _ => await FetchRates(), null, 0, 10000);
        }

        #endregion

        #region Fetch Rates

        private async Task FetchRates()
        {
            try
            {
                Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
                List<FxRateRequest> requests = new List<FxRateRequest> {
                    new FxRateRequest("USD", new List<string> { "ILS" }),
                    new FxRateRequest("EUR", new List<string> { "ILS", "USD", "GBP" }),
                    new FxRateRequest("GBP", new List<string> { "ILS" })
                };
                foreach (FxRateRequest fxRateRequest in requests)
                {
                        // Fetch data from the FxRates API
                        HttpResponseMessage response = await _httpClient.GetAsync(fxRateRequest.requestURL);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Error fetching exchange rates. Status: {response.StatusCode}");
                        }

                        string responseBody = await response.Content.ReadAsStringAsync();
                        JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        FxRatesResponse ratesResponse = JsonSerializer.Deserialize<FxRatesResponse>(responseBody, options);
                        if (ratesResponse?.Success == false)
                        {
                            throw new Exception($"Error fetching exchange rates. Code: {ratesResponse?.Error?.Code}, Message: {ratesResponse?.Error?.Message}");
                        }

                        foreach (KeyValuePair<string, decimal> currency in ratesResponse.Rates)
                        {
                            string currencyPair = $"{ratesResponse.BaseCurrency}/{currency.Key}";
                            rates[currencyPair] = currency.Value;
                        }
                }

                await SaveRatesToFile(rates);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task SaveRatesToFile(Dictionary<string, decimal> rates)
        {
            try
            {
                string filePath = Path.Combine("DB", "exchange_rates.json");
                string json = JsonSerializer.Serialize(rates, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving rates to file: {ex.Message}");
            }
        }

        #endregion
    }
}
