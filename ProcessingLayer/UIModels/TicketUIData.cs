using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final.ProcessingLayer.UIModels
{
    internal class TicketUIData
    {
        public int TicketId { get; set; }
        public PersonUIData TicketOwner { get; set; }
        public List<RouteUIData> Routes { get; set; }
    }
}
