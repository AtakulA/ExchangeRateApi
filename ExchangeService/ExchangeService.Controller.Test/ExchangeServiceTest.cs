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

        #region GetExchangeRate
        [Fact]
        public void Get_ExchangeRate_Success()
        {
            GetExchangeRateModel request = new GetExchangeRateModel { From = "USD", To = "EUR" };
            ExchangeRateModel model = new ExchangeRateModel { From = "USD", To = "EUR", Rate = 0.91M,AcquiredAt = DateTime.Now, ExpiresAt = DateTime.Now.AddHours(1), IsExpired = false  };


            service.GetExchangeRate(request).Returns(model);

            var result = controller.GetExchangeRate(request);
            var okObject = (OkObjectResult)result;

            okObject.Value.Equals(model);
        }
        #endregion

        #region Trade
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
        public void Create_Trade_Failed()
        {
            ExchangeTradeRequest req = new ExchangeTradeRequest { UserId = 2, From = "ABC", To = "EUR", UserAmount = 1500 };


            service.Trade(req).Returns(1);

            var result = controller.Trade(req);
            var okObject = (OkObjectResult)result;

            Assert.False((int)okObject.Value == 2);
        }

        #endregion

        #region GetUserTrade
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

        #endregion

        #region GetUserTrades
        [Fact]
        public void Get_UserTrades_Success()
        {
            int userId = 1;
            List<ExchangeTradeModel> list = new List<ExchangeTradeModel>();

            var res = service.GetUserTrades(userId);

            if(res != null && res.Count > 0)
            {

                var result = controller.GetTrades(userId);
                var okObject = (OkObjectResult)result;

                Assert.Equal(okObject.Value, list);
            }
            else
            {
                var result = controller.GetTrades(userId);
                var notFoundObject = (NotFoundResult)result;

                var requiredResponse = new NotFoundResult();

                Assert.Equal(notFoundObject.StatusCode ,requiredResponse.StatusCode);
            }
        }
        #endregion

        #region GetTrades
        [Fact]
        public void Get_Trades_Success()
        {
            List<ExchangeTradeModel> list = new List<ExchangeTradeModel>();

            service.GetAllUsersTrades().Returns(list);

            var result = controller.GetTrades();
            var okObject = (OkObjectResult)result;

            Assert.Equal(okObject.Value, list);
        }
        #endregion

        #region WhenCanUserTrade
        [Fact]
        public void Get_WhenCanUserTrade_Success()
        {
            int userId = 1;
            int response = 0;
            service.WhenCanUserTrade(userId).Returns(response);

            var result = controller.WhenCanUserTrade(userId);
            var okObject = (OkObjectResult)result;

            okObject.Value.Equals(response);
        }


        
        #endregion

    }
}