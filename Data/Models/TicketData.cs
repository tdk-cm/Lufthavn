using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final.ProcessingLayer.Models
{
    internal class TicketData
    {
        public int TicketId { get; set; }
        public PersonData TicketPerson { get; set; }
        public List<RouteData> Routes { get; set; }
    }
}
