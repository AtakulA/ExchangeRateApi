using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Data
{
    public class ExchangeTrade
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExchangeRateId { get; set; }
        public decimal UserAmount { get; set; }
        public decimal ExchangedAmount { get; set; }
        public DateTime ExchangedAt { get; set; }

        public virtual ExchangeRate ExchangeRate { get; set; }
    }
}
