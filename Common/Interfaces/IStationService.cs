
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IStationService
    {
        Task ReceiveAirplaneToStartingStationsAsync(Flight flight);
        void RegisterToStationEvent(StationHandler onStationEvent);
    }
}
