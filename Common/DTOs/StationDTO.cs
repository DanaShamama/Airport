using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [Serializable]
    public class StationDTO
    {
        public int? Id { get; set; }
        public int Name { get; set; }
        public int? FlightDTOId { get; set; }
        public FlightDTO FlightDTO { get; set; }
    }
}
