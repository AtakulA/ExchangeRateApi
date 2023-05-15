using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain.Models.Api
{
    public class ApiRateResponse
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string,decimal> Rates { get; set; }
        public bool Success { get; set; }
        public int timestamp { get; set; }

    }
}
