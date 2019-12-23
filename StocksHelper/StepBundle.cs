using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    [Serializable]
    public class StepBundle
    {
        public List<Step> Steps { get; set; }

        public string tsetid { get; set; }
    }
}
