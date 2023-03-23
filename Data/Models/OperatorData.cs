using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final.ProcessingLayer.Models
{
    internal class OperatorData
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<RouteData> Routes { get; set; }
    }
}
