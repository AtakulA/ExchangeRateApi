using AutoMapper;
using ExchangeService.Data;
using ExchangeService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly ExchangeRatesDbContext context;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;

        public CurrencyExchangeService(ExchangeRatesDbContext _context, IMapper _mapper, IDistributedCache _cache)
        {
            context = _context;
            mapper = _mapper;
            cache = _cache;
        }


        #region RateMethods

        public ExchangeRateModel GetExchangeRate(GetExchangeRateModel request)
        {
            try
            {
                ExchangeRate rate = GetAvailableExchangeRate(request);

                return mapper.Map<ExchangeRateModel>(rate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private ExchangeRate GetAvailableExchangeRate(GetExchangeRateModel request)
        {
            try
            {
                ExchangeRate? rate = context.ExchangeRates.FirstOrDefault(x => x.From == request.From && x.To == request.To && !x.IsExpired);

                if (rate != null)
                {
                    if (CheckRate(rate))
                    {
                        rate.IsExpired = true;
                        context.Update(rate);

                        ExchangeRate newRate = GetNewRate(request);

                        newRate.ExpiresAt = DateTime.Now.AddMinutes(30);
                        newRate.IsExpired = false;

                        context.Add(newRate);
                        context.SaveChanges();

                        return newRate;
                    }

                    else
                        return rate;
                }
                else
                {
                    ExchangeRate newRate = GetNewRate(request);

                    newRate.ExpiresAt = DateTime.Now.AddMinutes(30);
                    newRate.IsExpired = false;

                    context.Add(newRate);
                    context.SaveChanges();

                    return newRate;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool CheckRate(ExchangeRate rate)
        {
            return rate.ExpiresAt < DateTime.Now;

        }


        // API CALL
        private ExchangeRate GetNewRate(GetExchangeRateModel request)
        {
            throw new NotImplementedException();    // WRITE API CALL HERE TO GET CURRENT RATE
        }


        #endregion

        #region TradeMethods


        public int Trade(ExchangeTradeRequest request)
        {
            try
            {
                if (CanUserTrade(request.UserId))
                {
                    GetExchangeRateModel getExchangeRateModel = new GetExchangeRateModel { From = request.From, To = request.To };
                    ExchangeRate rate = GetAvailableExchangeRate(getExchangeRateModel);

                    ExchangeTrade trade = new ExchangeTrade
                    {
                        UserId = request.UserId,
                        ExchangeRateId = rate.Id,
                        UserAmount = request.UserAmount,
                        ExchangedAmount = (request.UserAmount * rate.Rate),
                        ExchangedAt = DateTime.Now
                    };

                    context.Add(trade);
                    context.SaveChanges();

                    return trade.Id;
                }

                else
                {
                    throw new Exception("This user has reached the trade limit and cannot trade! (Max 10 trades in an hour)");
                }
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool CanUserTrade(int userId)
        {
            try
            {
                List<ExchangeTrade> lastTrades = context.ExchangeTrades.Where(x => x.UserId == userId && x.ExchangedAt > DateTime.Now.AddHours(-1)).ToList();

                return lastTrades == null || lastTrades.Count < 10;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }



        //RETURNS THE TIME THAT USER NEEDS BEFORE USER CAN TRADE (IN MINUTES)
        public int WhenCanUserTrade(int userId)
        {
            try
            {
                if (CanUserTrade(userId))
                    return 0;
                else
                {
                    DateTime earliestTrade = GetUsersEarliestTrade(userId);
                    TimeSpan timeSpan = DateTime.Now - earliestTrade;
                    return timeSpan.Minutes;
                }
            }

            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        private DateTime GetUsersEarliestTrade(int userId)
        {
            try
            {
                List<ExchangeTrade> last10Trades = context.ExchangeTrades.Where(x => x.UserId == userId && x.ExchangedAt > DateTime.Now.AddHours(-1)).ToList();

                return last10Trades.OrderBy(x => x.ExchangedAt).First().ExchangedAt;
            }

            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        #endregion

        #region ListingMethods
        public ExchangeTradeModel GetUserTrade(int tradeId)
        {
            try
            {
                ExchangeTrade trade = context.ExchangeTrades.FirstOrDefault(x => x.Id == tradeId);

                return MapModel(trade);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<ExchangeTradeModel> GetUserTrades(int userId)
        {
            try
            {
                List<ExchangeTrade> trades = context.ExchangeTrades.Where(x => x.UserId == userId).Include(x => x.ExchangeRate).ToList();

                return MapModelList(trades);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public List<ExchangeTradeModel> GetAllUsersTrades()
        {
            try
            {
                List<ExchangeTrade> trades = context.ExchangeTrades.Include(x => x.ExchangeRate).ToList();

                if (trades.Count == 0)
                    return new List<ExchangeTradeModel>();

                else
                {
                    var orderedList = trades.OrderBy(x => x.UserId).ThenByDescending(x => x.ExchangedAt).ToList();

                    return MapModelList(orderedList);
                }
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private List<ExchangeTradeModel> MapModelList(List<ExchangeTrade> trades)
        {
            List<ExchangeTradeModel> tradeList = new List<ExchangeTradeModel>();

            foreach (ExchangeTrade trade in trades)
            {
                tradeList.Add(MapModel(trade));
            }

            return tradeList;
        }
        private ExchangeTradeModel MapModel(ExchangeTrade trade)
        {
            if (trade == null || trade.Id == 0)
                return new ExchangeTradeModel();

            ExchangeTradeModel model = new ExchangeTradeModel
            {
                UserId = trade.UserId,
                From = trade.ExchangeRate.From,
                To = trade.ExchangeRate.To,
                Rate = trade.ExchangeRate.Rate,
                UserAmount = trade.UserAmount,
                ExchangedAmount = trade.ExchangedAmount,
                ExchangedAt = trade.ExchangedAt
            };

            return model;
        }

        #endregion
    }
}
