using Lufthavn_Final.Data;
using Lufthavn_Final.ProcessingLayer.Models;
using Lufthavn_Final.ProcessingLayer.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final.ProcessingLayer
{
    internal class AirportProcessor
    {
        DataProcessor _dataProcessor;

        public AirportProcessor()
        {
             _dataProcessor = new DataProcessor();
        }

        public List<AirportUIData> GetAirports()
        {
            List<AirportData> airports = _dataProcessor.GetAirports();

            List<AirportUIData> returnList = new List<AirportUIData>();

            foreach(AirportData adata in airports)
            {
                returnList.Add(new AirportUIData()
                {
                    AirportName = adata.FullName,
                    AirportIataCode = adata.IataCode
                });
            }

            return returnList;
        }

        public List<OperatorUIData> GetOperators()
        {
            List<OperatorUIData> returnList = new List<OperatorUIData>();
            List<OperatorData> fromDb = _dataProcessor.GetOperators();

            foreach (OperatorData op in fromDb)
            {
                returnList.Add(new OperatorUIData()
                {
                    OperatorName = op.CompanyName,
                });
            }

            return returnList;
        }

        public List<PersonUIData> GetPeople()
        {
            List<PersonUIData> returnList = new List<PersonUIData>();
            List<PersonData> fromDb = _dataProcessor.GetPeople();

            foreach (PersonData op in fromDb)
            {
                returnList.Add(new PersonUIData()
                {
                    PersonName = op.PersonName,
                    PersonEmail = op.PersonEmail
                });
            }

            return returnList;
        }

        //
        // OLD
        //



        public List<RouteUIData> GetRoutes()
        {
            var routes = _dataProcessor.GetRoutes();
            List<RouteUIData> returnList = new List<RouteUIData>();

            foreach(RouteData r in routes)
            {
                RouteUIData rd = new RouteUIData();
                rd.DepartureIata = r.DepartureIata;
                rd.LandingIata = r.LandingIata;
                rd.Owner = MapOwner(r.Owner);
                rd.Operator = MapOwner(r.Operator);
                rd.RouteId = r.RouteId;
                returnList.Add(rd);
            };

            return returnList;
        }

        public List<TicketUIData> GetTickets()
        {
            var tickets = _dataProcessor.GetTickets();
            List<TicketUIData> returnList = new List<TicketUIData>();

            foreach(TicketData t in tickets)
            {
                TicketUIData rd = new TicketUIData();
                rd.TicketId = t.TicketId;
                rd.TicketOwner = MapPersonToUI(t.TicketPerson);
                rd.Routes = MapRoutes(t.Routes);
                returnList.Add(rd);
             }
            return returnList;
        }

        //
        // Add
        //

        public void AddTicket(TicketUIData ticket)
        {            
            _dataProcessor.AddTicket(MapTicket(ticket));
        }

        public void AddRoute(RouteUIData route)
        {
            try
            {
                _dataProcessor.AddRoute(MapFromRouteUI(route));
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                throw;
            }

        }

        public void AddAirport(AirportUIData airport)
        {
            AirportData port = new AirportData();
            port.FullName = airport.AirportName;
            port.IataCode = airport.AirportIataCode;

            _dataProcessor.AddAirport(port);
        }

        //
        // Linqs
        //

        public Ticket GetTicketData(int ticketId)
        {
            return _dataProcessor.GetTicketData(ticketId);
        }

        private List<RouteUIData> MapRoutes(List<RouteData> input)
        {
            List<RouteUIData> returnList = new List<RouteUIData>();

            foreach(RouteData r in input)
            {
                returnList.Add(MapRoute(r));
            }

            return returnList;
        }

        private RouteUIData MapRoute(RouteData input)
        {
            RouteUIData r = new RouteUIData();

            r.DepartureIata = input.DepartureIata;
            r.LandingIata = input.LandingIata;
            r.RouteId = input.RouteId;
            r.Owner = MapOperator(input.Owner);
            r.Operator = MapOperator(input.Operator);

            return r;
        }

        private RouteData MapFromRouteUI(RouteUIData input)
        {
            RouteData r = new RouteData();

            r.DepartureIata = input.DepartureIata;
            r.LandingIata = input.LandingIata;
            r.RouteId = input.RouteId;
            r.Owner = MapOwnerUI(input.Owner);
            r.Operator = MapOwnerUI(input.Operator);

            return r;
        }

        private List<RouteData> MapFromRoutesUI(List<RouteUIData> input)
        {
            List<RouteData> returnList = new List<RouteData>();

            foreach(RouteUIData rui in input)
            {
                RouteData rd = new RouteData();
                rd.DepartureIata = rui.DepartureIata;
                rd.LandingIata = rui.LandingIata;
                rd.RouteId = rui.RouteId;
                rd.Owner = MapOwnerUI(rui.Owner);
                rd.Operator = MapOwnerUI(rui.Operator);
                returnList.Add(rd);
            }

            return returnList;
        }

        private OperatorUIData MapOperator(OperatorData i)
        {
            OperatorUIData r = new OperatorUIData();
            r.OperatorId = i.CompanyId;
            r.OperatorName = i.CompanyName;
            return r;
        }

        private OperatorUIData MapOwner(OperatorData input)
        {
            return new OperatorUIData()
            {
                OperatorId = input.CompanyId,
                OperatorName = input.CompanyName
            };
        }

        private OperatorData MapOwnerUI(OperatorUIData input)
        {
            return new OperatorData()
            {
                CompanyId = input.OperatorId,
                CompanyName= input.OperatorName
            };
        }

        private PersonData MapPerson(PersonUIData p)
        {
            PersonData pData = new PersonData();
            pData.PersonEmail = p.PersonEmail;
            pData.PersonName = p.PersonName;

            return pData;
        }

        private PersonUIData MapPersonToUI(PersonData p)
        {
            PersonUIData ret = new PersonUIData();
            ret.PersonEmail = p.PersonEmail;
            ret.PersonName = p.PersonName;
            return ret;
        }

        private TicketData MapTicket(TicketUIData input)
        {
            TicketData td = new TicketData();
            td.TicketId = input.TicketId;
            td.TicketPerson = MapPerson(input.TicketOwner);
            td.Routes = MapFromRoutesUI(input.Routes);

            return td;
        }
    }
}
