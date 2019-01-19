using AirportServer.Hubs;
using Common.Convertors;
using Common.DTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportServer.Utils
{
    public class HubHelper
    {
        IHubContext _hub = GlobalHost.ConnectionManager.GetHubContext<AirportHub>();

        public void UpdateAirportState(AirportState airportState)
        {
            AirportStateDTO airportStateDTO = FromDtoToModel.FromModelToDtoAirportState(airportState);
            _hub.Clients.All.UpdateAirportState(airportStateDTO);
        }

        public void UpdateFutureFlights(FlightDTO flightDTO)
        {
            _hub.Clients.All.UpdateFutureFlights(flightDTO);
        }

        public HubHelper(IAirportManager airportManager)
        {
            airportManager.RegisterAirportStateEvent(UpdateAirportState);
            airportManager.RegisterFutureFlightsEvent(UpdateFutureFlights);
        }
    }
}