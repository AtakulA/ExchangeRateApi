using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain
{
    public class ExchangeTradeRequest
    {
        public int UserId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal UserAmount { get; set; }
    }
}
