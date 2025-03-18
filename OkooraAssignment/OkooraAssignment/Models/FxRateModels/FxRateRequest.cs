namespace OkooraAssignment.Models.FxRateModels
{
    public class FxRateRequest
    {
        public string requestURL { get; private set; }

        public FxRateRequest(string baseCurrency, List<string> currencies) 
        {
            requestURL = $"https://api.fxratesapi.com/latest?base={baseCurrency}&currencies={string.Join(',', currencies)}&resolution=1m&amount=1&places=6&format=json";
        }
    }
}
