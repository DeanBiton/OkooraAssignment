using System.Text.Json;

namespace OkooraAssignment.Services
{
    public class RatePrinterService
    {
        public RatePrinterService() {}

        public async Task<Dictionary<string, decimal>?> GetSavedRatesFromFile()
        {
            try
            {
                string filePath = Path.Combine("DB", "exchange_rates.json");
                if (!System.IO.File.Exists(filePath))
                {
                    return null; // No saved rates found
                }

                string json = await System.IO.File.ReadAllTextAsync(filePath);
                Dictionary<string, decimal>? savedRates = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);

                return savedRates;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading saved rates from file: {ex.Message}");
                return null;
            }
        }
    }
}
