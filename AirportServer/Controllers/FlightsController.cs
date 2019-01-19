using AirportServer.Utils;
using AirportServerBL;
using Common.DTOs;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirportServer.Controllers
{
    public class FlightsController : ApiController
    {
        private static readonly IAirportManager _airportManager;
        private static HubHelper _hubHelper;

        static FlightsController()
        {
            if (_airportManager == null)
            {
                _airportManager = new AirportManager();
            }
            if (_hubHelper == null)
            {
                _hubHelper = new HubHelper(_airportManager);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]FlightDTO newAirplane)
        {
            _airportManager.ReceiveAirplaneFromControler(newAirplane);
            return Ok();
        }
    }
}
