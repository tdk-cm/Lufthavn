using Lufthavn_Final.ProcessingLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lufthavn_Final.Data
{
    internal class DataProcessor
    {
        LufthavnEntitiesCon _context;
        public DataProcessor()
        {
            _context = new LufthavnEntitiesCon();
        }

        //
        // Standard gets
        //

        public List<AirportData> GetAirports()
        {
            List<AirportData> returnList = new List<AirportData>();
            var airports = _context.Airports;

            foreach(var airport in airports)
            {
                returnList.Add(new AirportData()
                {
                    FullName = airport.full_name,
                    IataCode = airport.iata_code
                });
            }

            
            return returnList;
        }

        public List<OperatorData> GetOperators()
        {
            List<OperatorData> returnList = new List<OperatorData>();
            var operators = _context.AirlineOperators;

            foreach(AirlineOperator op in operators){
                returnList.Add(new OperatorData()
                {
                    CompanyName = op.company_name,
                    CompanyId = op.company_id
                });
            }

            return returnList;
        }

        public List<PersonData> GetPeople()
        {
            List<PersonData> returnList = new List<PersonData>();
            var people = _context.People;

            foreach (Person op in people)
            {
                returnList.Add(new PersonData()
                {
                    PersonName = op.person_name,
                    PersonEmail = ""
                });
            }

            return returnList;
        }

        public List<RouteData> GetRoutes()
        {
            List<RouteData> returnList = new List<RouteData>();
            var routes = _context.Routes;

            foreach(Route r in routes)
            {
                returnList.Add(MapRouteData(r));
            }

            return returnList;
        }

        public List<TicketData> GetTickets()
        {
            List<TicketData> returnList = new List<TicketData>();

            var tickets = _context.Tickets;

            foreach(Ticket t in tickets)
            {
                returnList.Add(MapTicketData(t));
            }
            return returnList;
        }

        //
        // Add
        //

        public void AddTicket(TicketData ticket)
        {
            _context.Tickets.Add(MapTicket(ticket));
            _context.SaveChanges();
        }

        public void AddRoute(RouteData route)
        {
            try
            {

                _context.Routes.Add(MapRouteFromData(route));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                throw;
            }

        }

        public void AddAirport(AirportData airport)
        {
            Airport port = new Airport();
            port.full_name = airport.FullName;
            port.iata_code = airport.IataCode;
                
            _context.Airports.Add(port);
            _context.SaveChanges();
        }

        //
        // Linqs
        //

        public Ticket GetTicketData(int ticketId)
        {
            Ticket ticket = _context.Tickets.FirstOrDefault(x => x.ticket_id == ticketId);

            if(ticket is null)
            {
                return null;
            }

            ticket.Routes = _context.Tickets.Where(x => x.ticket_id == ticketId).SelectMany(i => i.Routes).ToList();

            ticket.Person = _context.People.FirstOrDefault(p => p.person_id == ticket.person_id);
            return ticket;
        }

        private Ticket MapTicket(TicketData tdata)
        {
            Ticket t = new Ticket();
            t.Person = MapPerson(tdata.TicketPerson);
            t.Routes = MapRoutesFromData(tdata.Routes);
            return t;
        }

        private Person MapPerson(PersonData p)
        {
            return new Person()
            {
                person_id = 0,
                person_name = p.PersonName                
            };
        }

        private List<Route> MapRoutesFromData(List<RouteData> routes)
        {
            List<Route> returnList = new List<Route>();

            foreach (RouteData r in routes)
            {
                returnList.Add(MapRouteFromData(r));
            }

            return returnList;
        }

        private Route MapRouteFromData(RouteData route)
        {
            Route r = new Route();
 

            r.router_operator = route.Operator.CompanyId;
            r.route_owner = route.Owner.CompanyId;
            r.AirlineOperator = MapOperatorData(route.Operator);
            r.AirlineOperator1 = MapOperatorData(route.Owner);
            r.from_iata = route.DepartureIata;
            r.to_iata = route.LandingIata;

            return r;
          
        }

        private AirlineOperator MapOperatorData(OperatorData op)
        {
            AirlineOperator o = new AirlineOperator();

            o.company_id = op.CompanyId;
            o.company_name = op.CompanyName;
            return o;
        }

        private OperatorData MapOperator(AirlineOperator input)
        {
            return new OperatorData()
            {
                CompanyName = input.company_name,
                CompanyId = input.company_id
            };
        }

        private AirportData MapAirportData(Airport input)
        {
            return new AirportData()
            {
                FullName = input.full_name,
                IataCode= input.iata_code
            };
        }

        private PersonData MapPersonData(Person input)
        {
            return new PersonData()
            {
                PersonName = input.person_name,
                PersonEmail= ""
            };
        }

        private RouteData MapRouteData(Route input)
        {
            return new RouteData()
            {
                RouteId = input.route_id,
                DepartureIata= input.from_iata,
                LandingIata = input.to_iata,
                Owner = MapOperator(input.AirlineOperator),
                Operator = MapOperator(input.AirlineOperator1)

            };
        }

        private TicketData MapTicketData(Ticket input)
        {
            return new TicketData()
            {
                TicketId = input.ticket_id,
                TicketPerson = MapPersonData(input.Person),
                Routes = MapRoutes(input.Routes.ToList())
            };
        }

        private List<RouteData> MapRoutes(List<Route> routes)
        {
            List<RouteData> returnList = new List<RouteData>();

            foreach(Route r in routes)
            {
                returnList.Add(new RouteData()
                {
                    RouteId = r.route_id,
                    DepartureIata = r.from_iata,
                    LandingIata = r.to_iata,
                    Owner = MapOperator(r.AirlineOperator),
                    Operator = MapOperator(r.AirlineOperator1)
                });
            }

            return returnList;
        }

    }
}
