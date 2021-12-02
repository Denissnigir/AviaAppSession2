using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviaAppSession2.Model
{
    public partial class Schedules
    {
        public string GetDepartureAirport
        {
            get
            {
                var airport = Context._con.Airports.Where(p => p.ID == Routes.DepartureAirportID)
                                                   .FirstOrDefault();
                return airport.IATACode;
            }
        }

        public string GetArrivalAirport
        {
            get
            {
                var airport = Context._con.Airports.Where(p => p.ID == Routes.ArrivalAirportID)
                                                   .FirstOrDefault();
                return airport.IATACode;
            }
        }

        public string GetEconomPrice
        {
            get
            {
                decimal price = EconomyPrice;
                return Math.Floor(price)
                           .ToString();
            }
        }

        public string GetBusinessPrice
        {
            get
            {
                decimal price = (EconomyPrice / 100) * 135;
                return Math.Floor(price)
                           .ToString();
            }
        }

        public string GetFirstClassPrice
        {
            get
            {
                decimal price = (((EconomyPrice / 100) * 135) / 100) * 130;
                return Math.Floor(price)
                           .ToString();
            }
        }

        public string GetColor
        {
            get
            {
                if (!Confirmed)
                {
                    return "Red";
                }
                else
                {
                    return "White";
                }
            }
        }

        public string GetFrom
        {
            get
            {
                string from = $"From: {GetDepartureAirport}";
                return from;
            }
        }

        public string GetTo
        {
            get
            {
                string to = $"To: {GetArrivalAirport}";
                return to;
            }
        }

        public string GetAircraft
        {
            get
            {
                string aircraft = $"Aircraft: {Aircrafts.Name}";
                return aircraft;
            }
        }
    }
}
