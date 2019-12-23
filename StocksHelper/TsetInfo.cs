using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public class TsetInfo
    {
        public TsetInfo()
        {
            LastTick = new Tick();
        }

        public Tick LastTick { get; set; }
        public double IndividualBuyVolume { get; set; }
        public double IndividualBuyNumberOfTrades { get; set; }
        public double IndividualSellVolume { get; set; }
        public double IndividualSellNumberOfTrades { get; set; }
        public double InstitutionBuyVolume { get; set; }
        public double InstitutionBuyNumberOfTrades { get; set; }
        public double InstitutionSellVolume { get; set; }
        public double InstitutionSellNumberOfTrades { get; set; }

        public double OverallNumberOfShares { get; set; }
        //public double 

    }

    
}
