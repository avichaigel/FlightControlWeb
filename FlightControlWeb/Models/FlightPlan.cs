using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        private int passengers;
        private string company_name;
        private InitialLocation initialLocation;
        private List<Segment> segments;
    }
}
