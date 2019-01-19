using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [Serializable]
    public class AirportStateDTO
    {
        public List<KeyValuePair<int, string>> CurrentStationsState { get; set; }
        public DateTime CurrentDateTime { get; set; }

        public AirportStateDTO()
        {
            CurrentStationsState = new List<KeyValuePair<int, string>>();
        }
    }
}
