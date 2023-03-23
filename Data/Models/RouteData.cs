using Lufthavn_Final.ProcessingLayer.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final.ProcessingLayer.Models
{
    internal class RouteData
    {
        public int RouteId { get; set; }
        public string DepartureIata { get; set; }
        public string LandingIata { get; set; }
        public OperatorData Owner { get; set; }
        public OperatorData Operator { get; set; }
    }
}
