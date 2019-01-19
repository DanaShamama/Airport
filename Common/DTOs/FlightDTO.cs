using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [Serializable]
    public class FlightDTO
    {
        public int? Id { get; set; }
        public bool IsDeparture { get; set; }
        public string FlightGuidAndType { get; set; }
        public DateTime StartTime { get; set; }
    }
}
