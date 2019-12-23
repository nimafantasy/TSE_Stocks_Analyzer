using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StocksHelper
{
    [Serializable]
    public class Step
    {
        public string tick { get; set; }

        public string tsetid { get; set; }

        public string b1number { get; set; }
        public string b1volume { get; set; }
        public string b1price { get; set; }
        public string b2number { get; set; }
        public string b2volume { get; set; }
        public string b2price { get; set; }
        public string b3number { get; set; }
        public string b3volume { get; set; }
        public string b3price { get; set; }

        public string s1number { get; set; }
        public string s1volume { get; set; }
        public string s1price { get; set; }
        public string s2number { get; set; }
        public string s2volume { get; set; }
        public string s2price { get; set; }
        public string s3number { get; set; }
        public string s3volume { get; set; }
        public string s3price { get; set; }

        public string b_ind_vol { get; set; }
        public string b_ind_number { get; set; }

        public string b_ins_vol { get; set; }
        public string b_ins_number { get; set; }

        public string s_ind_vol { get; set; }
        public string s_ind_number { get; set; }

        public string s_ins_vol { get; set; }
        public string s_ins_number { get; set; }

        public string daily_range_max { get; set; }
        public string daily_range_min { get; set; }

        public string yesterday_price { get; set; }
        public string lateset_trade_price { get; set; }
        public string close { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string vol { get; set; }
        public string number_of_trades { get; set; }
        public string value_of_trades { get; set; }
        public string date { get; set; }
    }
}
