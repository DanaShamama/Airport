using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [Serializable]
    public class FlightsHistoryDTO
    {
        public int? Id { get; set; }
        public int? FlightDTOId { get; set; }
        public FlightDTO FlightDTO { get; set; }
        public int? StationDTOId { get; set; }
        public StationDTO StationDTO { get; set; }
        public DateTime EnterStationTime { get; set; }
        public DateTime ExitStationTime { get; set; }
    }
}
