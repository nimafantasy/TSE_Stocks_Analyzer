using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper.Data
{
    public static class Constants
    {
        public static string TSETMCOnlineInfo { get { return @"http://www.tsetmc.com/tsev2/data/instinfofast.aspx?i={0}&c=34+"; } }
        public static string TSETMCOnlineCodalInfo { get { return @"http://www.tsetmc.com/Loader.aspx?ParTree=151311&i={0}"; } }
        public static string TSETMCHistoricPossessionInfo { get { return @"http://www.tsetmc.com/tsev2/data/clienttype.aspx?i={0}"; } }

        public static int ChartCellWidth { get { return 200; } }

        public static double AcceptableValueOfTradeMinimum { get { return 2000000000; } }
        public static double AcceptableRallyStreakMinimum { get { return 0.10; } }

        public static string XmlMarketData { get { return @"..\..\Data\Market.xml"; } }

        public static string XmlChosenData { get { return @"..\..\Data\ChosenOnes.xml"; } }

        public static string OnlineDataLocalPath { get { return @"..\..\Data\Online\"; } }
        public static string OnlineDataRemotePath { get { return @"\\{0}\c\Visual Studio 2015\Projects\StocksHelper\StocksHelper\Data\Online\"; } }
        public static string OnlineDataFileName { get { return "{0}.zip"; } }

        public static string XmlCriteriaData { get { return @"..\..\Data\Criteria.xml"; } }

        public static string RawFilesPath { get { return @"..\..\Data\Stocks\TSE\Unsortedfiles"; } }

        public static string MarketIndexFileName { get { return "IRX6XTPI0009.csv"; } }
        public static string MarketIndexName { get { return "شاخص کل"; } }
        public static string IndustryIndexFileName { get { return "IRX6XSNT0009.csv"; } }
        public static string IndustryIndexName { get { return "شاخص صنعت"; } }
        public static string Big30IndexFileName { get { return "IRX6XS300003.csv"; } }
        public static string Big30IndexName { get { return "شاخص 30 شرکت بزرگ"; } }
        public static string Big50IndexFileName { get { return "IRX6XWAI0001.csv"; } }
        public static string Big50IndexName { get { return "شاخص 50 شرکت بزرگ"; } }
        public static string MostActive50IndexFileName { get { return "IRX6XSLC0000.csv"; } }
        public static string MostActive50IndexName { get { return "شاخص 50 شرکت فعالتر"; } }
        public static string FirstMarketIndexFileName { get { return "IRX6XTAL0001.csv"; } }
        public static string FirstMarketIndexName { get { return "شاخص بازار اول"; } }
        public static string FloatingIndexFileName { get { return "IRX6XAFF0005.csv"; } }
        public static string FloatingtIndexName { get { return "شاخص آزاد شناور"; } }

        public static int SweepingStep { get { return 3; } }
        public static int SweepingWindow { get { return 7; } }

        public static double ClosenessThreshold { get { return 0.015; } }
        public static int ChartExtermumInitialWeight { get { return 20; } }
        public static int ChartExtermumReapeatedMeetWeightToBeAdded { get { return 2; } }
        public static int ChartExtermumNegligenceMultiplingFactor { get { return 1; } }


        public static double ChartMinimumStrength { get { return 0; } }

        public static TimeSpan MarketOpen { get { return new TimeSpan(8, 55, 0); } }
        public static TimeSpan MarketClose { get { return new TimeSpan(12, 35, 0); } }
    }
}
