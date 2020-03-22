namespace WalletManager.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WalletManager.API.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("convert")]
        public async Task<ActionResult<decimal>> Convert(string inCurrencyName, string outCurrencyName, decimal amount)
        {
            decimal result;

            try
            {
                result = await _currencyService.ConvertCurrency(inCurrencyName, outCurrencyName, amount);
            }
            catch (CurrencyServiceException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }
    }
}
