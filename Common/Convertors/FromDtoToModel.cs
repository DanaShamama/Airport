using Common.DTOs;
using Common.Enums;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Convertors
{
    public class FromDtoToModel
    {
        public static Flight FromAirplaneDtoToAirplaneModel(FlightDTO flightDTO)
        {
            FlightType flightType = new FlightType();
            if (flightDTO.IsDeparture)
            {
                flightType = FlightType.Departure;
            }
            else
            {
                flightType = FlightType.Arrival;
            }
            return new Flight { Guid = flightDTO.FlightGuidAndType, StartTime = flightDTO.StartTime, FlightType = flightType };
        }

        public static AirportStateDTO FromModelToDtoAirportState(AirportState airportState)
        {
            return new AirportStateDTO { CurrentDateTime = airportState.CurrentTime, CurrentStationsState = airportState.CurrentStationsState };
        }

        public static StationDTO FromStationModelToStationDto(KeyValuePair<int, string> receiveStation, FlightDTO flightDTO)
        { 
            return new StationDTO { FlightDTO = flightDTO, Name = receiveStation.Key};
        }
    }
}
