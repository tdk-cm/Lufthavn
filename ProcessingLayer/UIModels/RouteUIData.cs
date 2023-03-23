using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavn_Final.ProcessingLayer.UIModels
{
    internal class RouteUIData
    {
        public int RouteId { get; set; }
        public string DepartureIata { get; set; }
        public string LandingIata { get; set; }
        public OperatorUIData Owner { get; set; }
        public OperatorUIData Operator { get; set; }

    }
}
