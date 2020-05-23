using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightItem
    {
        private string flight_id;
        private double longtitude;
        private double latitude;
        private int passengers;
        private string company_name;
        private string date_time;
        bool is_external = false;
    }
}
