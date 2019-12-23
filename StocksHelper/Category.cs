using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public class Category
    {
        public Category()
        {
            CategoryIndex = new Stock();
            Stocks = new List<Stock>();
        }
        public string Name { get; set; }

        public Stock CategoryIndex { get; set; }

        public List<Stock> Stocks { get; set; }

        public string FileName { get; set; }

        public bool HasIndex { get; set; }
    }
}
