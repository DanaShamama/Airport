using Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Collections
{
    namespace Common.Collections
    {
        public sealed class StationsCollection
        {
            private static StationsCollection _stationsCollection = null;
            private static ConcurrentBag<Station> _stationsCollectionBag = null;
            private static readonly object lockObject1 = new object();
            private static readonly object lockObject2 = new object();

            StationsCollection()
            {
            }

            public static StationsCollection CollcectionClassInstance
            {
                get
                {
                    lock (lockObject1)
                    {
                        if (_stationsCollection == null)
                        {
                            _stationsCollection = new StationsCollection();
                        }
                        return _stationsCollection;
                    }
                }
            }

            public static ConcurrentBag<Station> StationsCollectionBag
            {
                get
                {
                    lock (lockObject2)
                    {
                        if (_stationsCollectionBag == null)
                        {
                            _stationsCollectionBag = new ConcurrentBag<Station>();
                        }
                        return _stationsCollectionBag;
                    }
                }
                set
                {
                    lock (lockObject2)
                    {
                        _stationsCollectionBag = value;
                    }
                }
            }

            public static Station GetStationById(int id)
            {
                var stationById = (from station in StationsCollectionBag
                                   where station.Id == id
                                   select station).FirstOrDefault<Station>();
                return stationById;
            }
        }
    }
}
