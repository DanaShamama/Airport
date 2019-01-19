using Common.Collections.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AirportState
    {
        public List<KeyValuePair<int, string>> CurrentStationsState { get; set; }
        public DateTime CurrentTime { get; set; }

        public AirportState()
        {
            CurrentStationsState = new List<KeyValuePair<int, string>>();
            foreach (var item in StationsCollection.StationsCollectionBag)
            {
                KeyValuePair<int, string> newPair = new KeyValuePair<int, string>(item.Id, null);
                CurrentStationsState.Add(newPair);
            }
        }
    }
}
