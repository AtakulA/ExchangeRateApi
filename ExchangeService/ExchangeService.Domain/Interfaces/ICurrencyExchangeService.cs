using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain
{
    public interface ICurrencyExchangeService
    {
        public ExchangeRateModel GetExchangeRate(GetExchangeRateModel request);
        public int Trade(ExchangeTradeRequest request);
        public ExchangeTradeModel GetUserTrade(int tradeId);

        public int WhenCanUserTrade(int userId);

        public List<ExchangeTradeModel> GetUserTrades(int userId);

        public List<ExchangeTradeModel> GetAllUsersTrades();
    }
}
