using AutoMapper;
using ExchangeService.Data;
using ExchangeService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Services
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ExchangeRate, ExchangeRateModel>().ReverseMap();


        }
    }
}
