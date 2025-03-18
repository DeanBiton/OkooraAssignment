using Microsoft.AspNetCore.Mvc;
using OkooraAssignment.Services;

namespace OkooraAssignment.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly RatePrinterService _ratePrinterService;

        public ExchangeRateController(RatePrinterService ratePrinterService)
        {
            _ratePrinterService = ratePrinterService;
        }

        [HttpGet("ratePrinter")]
        public async Task<IActionResult> GetSavedRates([FromQuery] string currencyPair = null)
        {
            try
            {
                Dictionary<string, decimal> savedRates = await _ratePrinterService.GetSavedRatesFromFile();

                // If a specific currency pair is requested
                if (!string.IsNullOrEmpty(currencyPair))
                {
                    if (savedRates != null && savedRates.ContainsKey(currencyPair))
                    {
                        return Ok(new { currencyPair, rate = savedRates[currencyPair] });
                    }
                    else
                    {
                        return NotFound($"Rate for {currencyPair} not found.");
                    }
                }

                // Return all saved rates if no specific currency pair is requested
                return Ok(savedRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading saved exchange rates: {ex.Message}");
            }
        }

    }
}
