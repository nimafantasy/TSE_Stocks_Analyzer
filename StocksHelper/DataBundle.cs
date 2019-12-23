using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksHelper.Enum;

namespace StocksHelper
{
    public class DataBundle
    {

        public DataBundle(DataNames _name)
        {
            List = new List<double>();
            Dates = new List<DateTime>();
            Name = _name;
        }
        public DataNames Name { get; set; }
        public List<double> List { get; set; }
        public List<DateTime> Dates { get; set; }
        public double RecordHigh { get; set; }
        public double RecordLow { get; set; }
        public double Average { get; set; }

        public double GetValueAt(DateTime dt)
        {
            int ind = Dates.IndexOf(dt);
            if (ind < 0)
                return List.Last();
            return List[ind];
        }


    }
}
