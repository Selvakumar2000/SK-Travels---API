using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKTravelsApp.BusinessServices;
using SKTravelsApp.Helpers;
using SKTravelsApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelsController : BaseAPIController
    {
        private readonly TokenService _tokenService;
        private readonly EmailSender _emailSender;
        private readonly TravelsService _travelService;
        public TravelsController(TokenService tokenService, EmailSender emailSender, TravelsService travelService)
        {
            _tokenService = tokenService;
            _emailSender = emailSender;
            _travelService = travelService;
        }

        [HttpPost("save/traveldays")]
        public Dictionary<string, object> SaveTravelDays(TravelDaysDto travelDays)
        {
            try
            {
                Dictionary<string, object> output = _travelService.SaveTravelDays(travelDays);
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("get/traveldays")]
        public Dictionary<string, object> GetTravelDays(TravelDaysDto travelDays)
        {
            try
            {
                Dictionary<string, object> output = _travelService.GetTravelDays(travelDays);
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("save/travelcities")]
        public Dictionary<string, object> SaveTravelCities(VisitingCitiesDto travelCities)
        {
            try
            {
                Dictionary<string, object> output = _travelService.SaveTravelCities(travelCities);
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
