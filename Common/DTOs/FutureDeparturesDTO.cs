using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [Serializable]
    public class FutureDeparturesDTO
    {
        public int? Id { get; set; }
        public int? FlightDTOId { get; set; }
        public FlightDTO FlightDTO { get; set; }
        public bool IsEntered { get; set; }
    }
}
