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
        LufthavnEntities10 _context;
        public DataProcessor()
        {
            _context = new LufthavnEntities10();
        }

        //
        // Standard gets
        //

        public List<Airport> GetAirports()
        {

            var airports = _context.Airports;
            return airports.ToList();
        }

        public List<AirlineOperator> GetOperators()
        {
            var operators = _context.AirlineOperators;
            return operators.ToList();
        }

        public List<Person> GetPeople()
        {
            var people = _context.People;
            return people.ToList();
        }

        public List<Route> GetRoutes()
        {
            var routes = _context.Routes;
            return routes.ToList();
        }

        public List<Ticket> GetTickets()
        {
            var tickets = _context.Tickets;
            return tickets.ToList();
        }

        //
        // Add
        //

        public void AddTicket(Ticket ticket)
        {
            List<Ticket> tickets = _context.Tickets.ToList();

            int highest = 0;

            for (int i = 0; i < tickets.Count; i++)
            {
                if (tickets[i].ticket_id > highest)
                {
                    highest = tickets[i].ticket_id;
                }
            }

            ticket.ticket_id = highest + 1;
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void AddRoute(Route route)
        {
            try
            {
                List<Route> routes = _context.Routes.ToList();

                int highest = 0;

                for (int i = 0; i < routes.Count; i++)
                {
                    if (routes[i].route_id > highest)
                    {
                        highest = routes[i].route_id;
                    }
                }

                route.route_id= highest + 1;

                _context.Routes.Add(route);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                throw;
            }

        }

        public void AddAirport(Airport airport)
        {
            _context.Airports.Add(airport);
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

    }
}
