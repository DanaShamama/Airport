using Common.Interfaces;
using Common.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportServer.Hubs
{
    [HubName("AirportHub")]
    public class AirportHub : Hub
    {
   
    }
}