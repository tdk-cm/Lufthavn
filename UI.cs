using Lufthavn_Final.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final
{
    internal class UI
    {
        DataProcessor _processor = new DataProcessor();

        public void Start()
        {
            while (true)
            {
                Dictionary<ConsoleKey, string> choices = new Dictionary<ConsoleKey, string>();
                Console.Clear();
                choices.Add(ConsoleKey.D1, "Get Data");
                choices.Add(ConsoleKey.D2, "Add Data");

                ConsoleKey resultKey = ConsoleTools.GetUserChoices(choices);

                switch (resultKey)
                {
                    case ConsoleKey.D1:
                        ProcessGetData();
                        break;

                    case ConsoleKey.D2:
                        ProcessAddData();
                        break;

                    default:
                        break;
                }
            }
   
           
            Console.ReadKey();
        }

        private void ProcessAddData()
        {
            Dictionary<ConsoleKey, string> choices = new Dictionary<ConsoleKey, string>();
            Console.Clear();
            choices.Add(ConsoleKey.D1, "Create Ticket");
            choices.Add(ConsoleKey.D2, "Add Airport");
            choices.Add(ConsoleKey.D3, "Add Route");

            ConsoleKey resultKey = ConsoleTools.GetUserChoices(choices);

            switch (resultKey)
            {
                case ConsoleKey.D1:
                    CreateTicket();
                    break;

                case ConsoleKey.D2:
                    AddAirport();
                    break;

                case ConsoleKey.D3:
                    AddRoute();
                    break;
            }
        }

        private void AddRoute()
        {
            Route newRoute = new Route();
            // Load choices
            List<Airport> airports = _processor.GetAirports();
            List<AirlineOperator> operators = _processor.GetOperators();

            List<string> airportChoice = new List<string>();

            for (int i = 0; i < airports.Count; i++)
            {
                airportChoice.Add($"{airports[i].full_name} - {airports[i].iata_code}");
            }

            List<string> operatorChoices = new List<string>();

            for (int i = 0; i < operators.Count; i++)
            {
                operatorChoices.Add($"{operators[i].company_name}");
            }

            Console.WriteLine("Choose starting airport: ");
            Console.WriteLine();

            newRoute.from_iata = ConsoleTools.GetUserChoices<Airport>(airportChoice, airports).iata_code;

            Console.WriteLine("Choose landing airport: ");
            Console.WriteLine();

            newRoute.to_iata = ConsoleTools.GetUserChoices<Airport>(airportChoice, airports).iata_code;

            Console.WriteLine($"Choose Route Owner");

            newRoute.route_owner = ConsoleTools.GetUserChoices<AirlineOperator>(operatorChoices, operators).company_id;

            Console.WriteLine($"Choose Route Operator");

            newRoute.router_operator = ConsoleTools.GetUserChoices<AirlineOperator>(operatorChoices, operators).company_id;


            _processor.AddRoute(newRoute);

            Console.WriteLine("New route added!");
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
            
        }

        private void AddAirport()
        {
            Console.Clear();
            Console.WriteLine($"Airport Name:");
            string name = Console.ReadLine();

            bool goodIata = false;

            string iata = "";
            while (!goodIata)
            {
                Console.WriteLine("Airport Iata code:");
                iata = Console.ReadLine();

                if(iata.Length != 3)
                {
                    Console.WriteLine("Iata code has to be 3 letters long");
                }
                else
                {
                    goodIata = true;
                }
            }

            Airport newAirport = new Airport();
            newAirport.full_name = name;
            newAirport.iata_code = iata;
            _processor.AddAirport(newAirport);

        }

        private void CreateTicket()
        {
            Console.Clear();
            Console.WriteLine("Select Person: ");

            Dictionary<ConsoleKey, string> choices = new Dictionary<ConsoleKey, string>();

            List<Person> people = _processor.GetPeople();

            for (int i = 0; i < people.Count; i++)
            {
                choices.Add((ConsoleKey)49 + i, people[i].person_name);
            }

            ConsoleKey resultKey = ConsoleTools.GetUserChoices(choices);

            Person chosen = people[(int)resultKey - 49]; // Oh I like to live dangerously

            Console.WriteLine("Chosen: " + chosen.person_name);

            List<Route> chosenRoutes = new List<Route>();

            // Load Routes
            List<Route> routes = _processor.GetRoutes();

            // Load routes in choices
            Dictionary<ConsoleKey, string> routechoices = new Dictionary<ConsoleKey, string>();
            for (int i = 0; i < routes.Count; i++)
            {
                routechoices.Add((ConsoleKey)49 + i, $"{routes[i].from_iata} - {routes[i].to_iata}");
            }

            routechoices.Add(ConsoleKey.C, "Done");

            bool continueRoutes = true;
        

            while (continueRoutes)
            {
                ConsoleKey routeChoice = ConsoleTools.GetUserChoices(routechoices);

                if(routeChoice == ConsoleKey.C)
                {
                    continueRoutes = false;
                    break;
                }

                chosenRoutes.Add(routes[(int)routeChoice - 49]);
            }

            Ticket ticket = new Ticket();

            ticket.Person = chosen;
            ticket.Routes = chosenRoutes;

            _processor.AddTicket(ticket);

            Console.WriteLine("Ticket added. Press any key to go back");
            Console.ReadKey();
        }

        private void ProcessGetData()
        {
            Console.Clear();
            Dictionary<ConsoleKey, string> choices = new Dictionary<ConsoleKey, string>();

            choices.Add(ConsoleKey.D1, "Airports List");
            choices.Add(ConsoleKey.D2, "Tickets List");
            choices.Add(ConsoleKey.D3, "Routes List");

            ConsoleKey resultKey = ConsoleTools.GetUserChoices(choices);

            if (resultKey == ConsoleKey.D1)
            {
                Console.Clear();
                List<Airport> airports = _processor.GetAirports();
                foreach(Airport airport in airports)
                {
                    Console.WriteLine($"- Name: '{airport.full_name}', Iata: {airport.iata_code}");
                }

                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                return;
            }
            else if(resultKey == ConsoleKey.D2)
            {
                Console.Clear();
                List<Ticket> tickets = _processor.GetTickets();
                Dictionary<ConsoleKey, string> ticketsChoice = new Dictionary<ConsoleKey, string>();

                for (int i = 0; i < tickets.Count; i++)
                {
                    ticketsChoice.Add((ConsoleKey)49 + i, $"Ticket owner: {tickets[i].Person.person_name}");
                }

                Console.WriteLine();
                Console.WriteLine("Choose a ticket to see details: ");
                ConsoleKey key = ConsoleTools.GetUserChoices(ticketsChoice);

                Console.WriteLine();

                // I know this sucks. Just doing to show that it works
                var ticket = _processor.GetTicketData(tickets[(int)key - 49].ticket_id);

                Console.WriteLine($"Ticket ID: {ticket.ticket_id}");
                Console.WriteLine($"Owner: {ticket.Person.person_name}");
                Console.WriteLine("Routes:");
                foreach (Route route in ticket.Routes)
                {
                    Console.WriteLine($"From: {route.from_iata}, to: {route.to_iata}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;

            } 
            else if(resultKey == ConsoleKey.D3)
            {
                Console.Clear();
                Console.WriteLine("Routes:");
                Console.WriteLine();

                List<Route> routes = _processor.GetRoutes();

                foreach (var route in routes)
                {
                    Console.WriteLine($"- From: {route.from_iata}, To: {route.to_iata}. Owned by: {route.AirlineOperator.company_name}, Operated by: {route.AirlineOperator1.company_name}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to go back...");
                Console.ReadKey();
            }
        }


    }


}
