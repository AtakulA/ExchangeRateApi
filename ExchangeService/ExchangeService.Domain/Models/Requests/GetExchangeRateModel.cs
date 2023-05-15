using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain
{
    public class GetExchangeRateModel
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}
