using StocksHelper.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public class Criterion
    {
        public string Name { get; set; }
        public string id { get; set; }


        public int HeadPeriod { get; set; }     // e.g. 3 days   period in which great changes have occured
        public double HeadRangeMin { get; set; }    // min magnitude in inc/dec 
        public double HeadRangeMax { get; set; }    //  max magnitude in inc/dec
        public Slope HeadSlope { get; set; }   // positive,negetive,plateau
        public Amount HeadAverageAmount { get; set; }  // actual value
        public SRPosition HeadSRPosition { get; set; }
        

        public int TailPeriod { get; set; }     // e.g. 20 days     oeriod which leads to the head period
        public Slope TailSlope { get; set; }   // positive,negetive,plateau
        public double TailRangeMin { get; set; }    // min magnitude in inc/dec 
        public double TailRangeMax { get; set; }    //  max magnitude in inc/dec 
        public double TailPullbackDistribution { get; set; }   // opposite moves distribution percent. 0 means not a single consecutive opposite move. 1 means opposite moves could all be next to each other
        public double TailPullBackTolerance { get; set; }   // what percent of moves could be opposite. 0 means not a single opposite move. 1 means all could be opposite.
        public Amount TailAverageAmount { get; set; }

    }
}
