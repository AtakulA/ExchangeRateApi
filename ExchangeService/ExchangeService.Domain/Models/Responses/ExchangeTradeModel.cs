using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain
{
    public class ExchangeTradeModel
    {
        public int UserId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }
        public decimal UserAmount { get; set; }
        public decimal ExchangedAmount { get; set; }
        public DateTime ExchangedAt { get; set; }

    }
}
