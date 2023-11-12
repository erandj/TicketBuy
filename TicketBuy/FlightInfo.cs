using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBuy
{
    internal class FlightInfo
    {
        public static string GetFlightCode(Route route, FlightEntities context)
        {
            string code = "";
            code = context.AirlineCode
                                    .Where(x => x.airline_id == route.airline_id
                                                && x.type_id == context.CodeType.Where(type => type.id == 1).FirstOrDefault().id)
                                    .FirstOrDefault()
                                    .code;
            code += "-" + route.flight_number_numeric.ToString();

            return code;
        }

        public static string GetAirlineCode(Airline airline, FlightEntities context)
        {
            string code = "";
            code = context.AirlineCode
                                    .Where(x => x.airline_id == airline.id
                                                && x.type_id == context.CodeType.Where(type => type.id == 1).FirstOrDefault().id)
                                    .FirstOrDefault()
                                    .code;

            return code;
        }

        public static string GetAirportCode(Airport airport, FlightEntities context)
        {
            string code = "";
            code = context.AirportCode
                                    .Where(x => x.airport_id == airport.id
                                                && x.type_id == context.CodeType.Where(type => type.id == 1).FirstOrDefault().id)
                                    .FirstOrDefault()
                                    .code;

            return code;
        }
    }
}
