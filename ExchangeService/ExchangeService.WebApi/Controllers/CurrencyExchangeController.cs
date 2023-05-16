using ExchangeService.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExchangeService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly ICurrencyExchangeService service;
        private readonly ILogger<CurrencyExchangeController> logger;

        public CurrencyExchangeController(ICurrencyExchangeService _service, ILogger<CurrencyExchangeController> _logger)
        {
            service = _service;
            logger = _logger;
        }


        [HttpGet("GetExchageRate/from={from}&to={to}")]
        public IActionResult GetExchangeRate(string from, string to)
        {
            try
            {
                return Ok(service.GetExchangeRate(new GetExchangeRateModel { From = from,To = to}));
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        [HttpPost("Trade")]
        public IActionResult Trade([FromBody] ExchangeTradeRequest request)
        {
            try
            {
                return Ok(service.Trade(request));
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetTrade/{tradeId}")]
        public IActionResult GetTrade(int tradeId)
        {
            try
            {
                return Ok(service.GetUserTrade(tradeId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetTrades/{userId}")]
        public IActionResult GetTrades(int userId)
        {
            try
            {
                var list = service.GetUserTrades(userId);
                return list == null || list.Count == 0 ? NotFound(): Ok(list);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetTrades")]
        public IActionResult GetTrades()
        {
            try
            {
                return Ok(service.GetAllUsersTrades());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("WhenCanUserTrade/{userId}")]
        public IActionResult WhenCanUserTrade(int userId)
        {
            try
            {
                return Ok(service.WhenCanUserTrade(userId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
