using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBuy
{
    internal class FlightTicketItem
    {
        private int id;
        private string cost;
        private string baggageCost;

        private string inFlight;
        private string departureTime;
        private string departureCity;
        private string departureDate;
        private string arrivalTime;
        private string arrivalCity;
        private string arrivalDate;
        private string departureCode;
        private string arrivalCode;

        private string inFlight_r;
        private string departureTime_r;
        private string departureCity_r;
        private string departureDate_r;
        private string arrivalTime_r;
        private string arrivalCity_r;
        private string arrivalDate_r;
        private string departureCode_r;
        private string arrivalCode_r;

        public string Cost { get => cost; set => cost = value; }
        public string DepartureTime { get => departureTime; set => departureTime = value; }
        public string DepartureCity { get => departureCity; set => departureCity = value; }
        public string DepartureDate { get => departureDate; set => departureDate = value; }
        public string ArrivalTime { get => arrivalTime; set => arrivalTime = value; }
        public string ArrivalCity { get => arrivalCity; set => arrivalCity = value; }
        public string ArrivalDate { get => arrivalDate; set => arrivalDate = value; }
        public string DepartureCode { get => departureCode; set => departureCode = value; }
        public string ArrivalCode { get => arrivalCode; set => arrivalCode = value; }
        public string BaggageCost { get => baggageCost; set => baggageCost = value; }
        public string InFlight { get => inFlight; set => inFlight = value; }

        public string InFlight_r { get => inFlight_r; set => inFlight_r = value; }
        public string DepartureTime_r { get => departureTime_r; set => departureTime_r = value; }
        public string DepartureCity_r { get => departureCity_r; set => departureCity_r = value; }
        public string DepartureDate_r { get => departureDate_r; set => departureDate_r = value; }
        public string ArrivalTime_r { get => arrivalTime_r; set => arrivalTime_r = value; }
        public string ArrivalCity_r { get => arrivalCity_r; set => arrivalCity_r = value; }
        public string ArrivalDate_r { get => arrivalDate_r; set => arrivalDate_r = value; }
        public string DepartureCode_r { get => departureCode_r; set => departureCode_r = value; }
        public string ArrivalCode_r { get => arrivalCode_r; set => arrivalCode_r = value; }
        public int Id { get => id; set => id = value; }
    }
}
