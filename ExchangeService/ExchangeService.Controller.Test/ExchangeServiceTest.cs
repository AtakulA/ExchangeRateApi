using ExchangeService.Domain;
using ExchangeService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExchangeService.Controller.Test
{
    public class CurrencyExchangeServiceTest
    {
        private readonly CurrencyExchangeController controller;
        private readonly ICurrencyExchangeService service = Substitute.For<ICurrencyExchangeService>();
        private readonly ILogger<CurrencyExchangeController> logger = Substitute.For<ILogger<CurrencyExchangeController>>();
        public CurrencyExchangeServiceTest() { controller = new CurrencyExchangeController(service, logger); }

        [Fact]
        public void Create_Trade_Success()
        {
            ExchangeTradeRequest req = new ExchangeTradeRequest { UserId = 1, From = "USD", To = "EUR", UserAmount = 1500 };


            service.Trade(req).Returns(1);

            var result = controller.Trade(req);
            var okObject = (OkObjectResult)result;

            okObject.Value.Equals(1);
        }

        [Fact]
        public void Get_UserTrade_Success()
        {
            int req = 1;
            ExchangeTradeModel model = new ExchangeTradeModel { UserId = 1, UserAmount = 1500, ExchangedAt = DateTime.Now, From = "USD", To = "EUR", Rate = 0.91M, ExchangedAmount = 1365 };


            service.GetUserTrade(req).Returns(model);

            var result = controller.GetTrade(req);
            var okObject = (OkObjectResult)result;

            okObject.Value.Equals(model);
        }

        [Fact]
        public void Get_UserTrades_Failed()
        {
            int req = 1;
            List<ExchangeTradeModel> list = new List<ExchangeTradeModel>();

            var res = new NotFoundResult();

            service.GetUserTrades(req).Returns(list);

            var result = controller.GetTrade(req);
            var notFoundObject = (OkObjectResult)result;

            Assert.False(notFoundObject.Value == list);
        }
    }
}