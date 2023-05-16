using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Data
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }
        public DateTime AcquiredAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired { get; set; }

        public virtual IEnumerable<ExchangeTrade> ExchangeTrades { get; set; }
    }
}
