using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Flight : IFlight
    {
        public string Guid { get; set; }
        public FlightType FlightType { get; set; }
        public DateTime StartTime { get; set; }

        public async Task DelayAirplane()
        {
            Random random = new Random();
            int delay = random.Next(1000, 5000);
            await Task.Delay(delay);
        }
    }
}
