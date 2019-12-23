using StocksHelper.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace StocksHelper
{
    public class Entry
    {
        public string name { get; set; }
        public string desc { get; set; }
        public double close_change { get; set; }
        public double vol_change { get; set; }
        public double vt_change { get; set; }
        public double nt_change { get; set; }
        public double queue { get; set; }

        public double final_prcnt_3daysago { get; set; }
        public double final_prcnt_2daysago { get; set; }
        public double final_prcnt_1dayago { get; set; }
        public double fiftyday_basevol_surpass_percentage { get; set; }
        public double snd_last_streak_sum { get; set; }
        public int snd_last_streak_days { get; set; }
        public string snd_last_streak_friendly { get; set; }
        public double last_streak_sum { get; set; }
        public int last_streak_days { get; set; }
        public string last_streak_friendly { get; set; }
        public double today_streak { get; set; }
        public Stock stock { get; set; }
        public Category cat { get; set; }
        public Chart green_chart { get; set; }
        public Chart index_chart { get; set; }
        public Chart performance_chart { get; set; }
        public Chart ind_chart { get; set; }
        public Chart ins_chart { get; set; }
        public Chart ask_bid_chart { get; set; }
        public EntryMode mode { get; set; }
        public double AbsoluteFluctuation { get; set; }
    }
}
