using StocksHelper.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public class Tick
    {
        public DateTime Date { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public double ValueOfTrades { get; set; }

        public double NumberOfTrades { get; set; }

        public double YesterdayPrice { get; set; }

        public double Hammer { get; set; }

        public double Pendulum { get; set; }

        public double IndividualBuyVolume { get; set; }
        public double IndividualBuyNumberOfTrades { get; set; }
        public double IndividualSellVolume { get; set; }
        public double IndividualSellNumberOfTrades { get; set; }
        public double InstitutionBuyVolume { get; set; }
        public double InstitutionBuyNumberOfTrades { get; set; }
        public double InstitutionSellVolume { get; set; }
        public double InstitutionSellNumberOfTrades { get; set; }

        public double OverallNumberOfShares { get; set; }

    }

}
