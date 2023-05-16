using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain.Models.Api
{
    public class ApiCurrenciesResponse
    {
        public bool Success { get; set; }
        public Dictionary<string,string> Symbols { get; set; }
    }
}
