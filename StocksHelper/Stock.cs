using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksHelper.Enum;
using StocksHelper.Data;

namespace StocksHelper
{
    public class Stock
    {
        public string Name { get; set; }

        // *******
        public double TotalShares { get; set; }
        public double BaseVolume { get; set; }
        public double OutstandingShares { get; set; }
        public double EPS { get; set; }
        //public 

        public bool IsActive { get; set; }
        public bool IsCompetent { get; set; }
        public string TSETID { get; set; }

        public TsetInfo OnlineInfo { get; set; }

        public List<string> FileNames { get; set; }
        public List<Tick> Ticks { get; set; }
        public Category ParentCategory { get; set; }

        public List<Extreme> Extremes { get; set; }
        public List<Step> Steps { get; set; }

        public DataBundle High { get; set; }
        public DataBundle Low { get; set; }
        public DataBundle Open { get; set; }
        public DataBundle Close { get; set; }
        public DataBundle Volume { get; set; }
        public DataBundle ValueOfTrades { get; set; }
        public DataBundle NumberOfTrades { get; set; }
        public DataBundle YesterdayPrice { get; set; }
        public DataBundle Pendulum { get; set; }
        public DataBundle Hammer { get; set; }

        public List<DataBundle> DataBundles { get; set; }

        public List<Property> PassedCriteria { get; set; }
        

        #region StartUp

        public Stock()
        {
            OnlineInfo = new TsetInfo();
            Ticks = new List<Tick>();
            Steps = new List<Step>();
            DataBundles = new List<DataBundle>();
            PassedCriteria = new List<Property>();
            Extremes = new List<Extreme>();
            FileNames = new List<string>();
            High = new DataBundle(DataNames.High);
            Low = new DataBundle(DataNames.Low);
            Open = new DataBundle(DataNames.Open);
            Close = new DataBundle(DataNames.Close);
            Volume = new DataBundle(DataNames.Volume);
            ValueOfTrades = new DataBundle(DataNames.ValueOfTrades);
            NumberOfTrades = new DataBundle(DataNames.NumberOfTrades);
            Pendulum = new DataBundle(DataNames.Pendulum);
            Hammer = new DataBundle(DataNames.Hammer);

            DataBundles.Add(High);
            DataBundles.Add(Low);
            DataBundles.Add(Open);
            DataBundles.Add(Close);
            DataBundles.Add(Volume);
            DataBundles.Add(ValueOfTrades);
            DataBundles.Add(NumberOfTrades);
            DataBundles.Add(Pendulum);
            DataBundles.Add(Hammer);

        }

        #endregion

        public void Init()
        {
            // calculate data based on the last tick
            Ticks = Ticks.OrderBy(o => o.Date).ToList();
            InitLists();
            LoadExtremes();

            if (IsActive)
            {
                double ninetydaytradevalueaverage = ValueOfTrades.List.Skip(ValueOfTrades.List.Count() - 90).Take(90).Average();
                int count = 0;
                Market market = new Market();
                var result = market.GetSubListD(Close,DateTime.Now,30)
                        .GroupBy(n => n < 0 ? ++count : count)
                        .Select(x => x.Sum(n => n > 0 ? n : 0));

                //Console.WriteLine(result.Max());

                if (ninetydaytradevalueaverage < Constants.AcceptableValueOfTradeMinimum || result.Max() < Constants.AcceptableRallyStreakMinimum)
                {
                    IsCompetent = false;
                }
                else
                {
                    IsCompetent = true;
                }
            }
            else
            {
                IsCompetent = false;
            }
            
        }

        private void InitLists()
        {
            //List<double> tmp = new List<double>();
            //List<double> tmp2 = new List<double>();
            foreach (Tick tck in Ticks)
            {
                High.List.Add(tck.High);
                Low.List.Add(tck.Low);
                Open.List.Add(tck.Open);
                Close.List.Add(tck.Close);
                Volume.List.Add(tck.Volume);
                ValueOfTrades.List.Add(tck.ValueOfTrades);
                NumberOfTrades.List.Add(tck.NumberOfTrades);

                double pen = Math.Max(Math.Round(tck.High - Math.Max(tck.Open, tck.Close), 2), 1);
                double ham = Math.Max(Math.Round(Math.Min(tck.Open, tck.Close) - tck.Low, 2), 1);

                Pendulum.List.Add(pen);
                Hammer.List.Add(ham);

                tck.Hammer = pen;
                tck.Pendulum = ham;

                High.Dates.Add(tck.Date);
                Low.Dates.Add(tck.Date);
                Open.Dates.Add(tck.Date);
                Close.Dates.Add(tck.Date);
                Volume.Dates.Add(tck.Date);
                ValueOfTrades.Dates.Add(tck.Date);
                NumberOfTrades.Dates.Add(tck.Date);
                Pendulum.Dates.Add(tck.Date);
                Hammer.Dates.Add(tck.Date);

                //tmp.Add(Math.Round(tck.ValueOfTrades / tck.NumberOfTrades,2));
                //tmp2.Add(Math.Round(tck.ValueOfTrades / tck.Close, 2));
            }

            try
            {
                High.RecordHigh = Math.Round(High.List.Max(), 2);
                High.RecordLow = Math.Round(High.List.Where(f => f != 0).Min(), 2);
                High.Average = Math.Round(High.List.Average(), 2);

                Low.RecordHigh = Math.Round(Low.List.Max(), 2);
                Low.RecordLow = Math.Round(Low.List.Where(f => f != 0).Min(), 2);
                Low.Average = Math.Round(Low.List.Average(), 2);

                Open.RecordHigh = Math.Round(Open.List.Max(), 2);
                Open.RecordLow = Math.Round(Open.List.Where(f => f != 0).Min(), 2);
                Open.RecordHigh = Math.Round(Open.List.Average(), 2);

                Close.RecordHigh = Math.Round(Close.List.Max(), 2);
                Close.RecordLow = Math.Round(Close.List.Where(f => f != 0).Min(), 2);
                Close.Average = Math.Round(Close.List.Average(), 2);

                Volume.RecordHigh = Math.Round(Volume.List.Max(), 2);
                Volume.RecordLow = Math.Round(Volume.List.Where(f => f != 0).Min(), 2);
                Volume.Average = Math.Round(Volume.List.Average(), 2);

                ValueOfTrades.RecordHigh = Math.Round(ValueOfTrades.List.Max(), 2);
                ValueOfTrades.RecordLow = Math.Round(ValueOfTrades.List.Where(f => f != 0).Min(), 2);
                ValueOfTrades.Average = Math.Round(ValueOfTrades.List.Average(), 2);

                NumberOfTrades.RecordHigh = Math.Round(NumberOfTrades.List.Max(), 2);
                NumberOfTrades.RecordLow = Math.Round(NumberOfTrades.List.Where(f => f != 0).Min(), 2);
                NumberOfTrades.Average = Math.Round(NumberOfTrades.List.Average(), 2);

                Pendulum.RecordHigh = Math.Round(Pendulum.List.Max(), 2);
                Pendulum.RecordLow = Math.Round(Pendulum.List.Where(f => f > 0).Min(), 2);
                Pendulum.Average = Math.Round(Pendulum.List.Where(f => f > 0).Average(), 2);

                Hammer.RecordHigh = Math.Round(Hammer.List.Max(), 2);
                Hammer.RecordLow = Math.Round(Hammer.List.Where(f => f > 0).Min(), 2);
                Hammer.Average = Math.Round(Hammer.List.Where(f => f > 0).Average(), 2);

            }
            catch(Exception ex)
            {
                High.RecordHigh = 0;
                High.RecordLow = 0;
                High.Average = 0;

                Low.RecordHigh = 0;
                Low.RecordLow = 0;
                Low.Average = 0;

                Open.RecordHigh = 0;
                Open.RecordLow = 0;
                Open.RecordHigh = 0;

                Close.RecordHigh = 0;
                Close.RecordLow = 0;
                Close.Average = 0;

                Volume.RecordHigh = 0;
                Volume.RecordLow = 0;
                Volume.Average = 0;

                ValueOfTrades.RecordHigh = 0;
                ValueOfTrades.RecordLow = 0;
                ValueOfTrades.Average = 0;

                NumberOfTrades.RecordHigh = 0;
                NumberOfTrades.RecordLow = 0;
                NumberOfTrades.Average = 0;

                Pendulum.RecordHigh = 0;
                Pendulum.RecordLow = 0;
                Pendulum.Average = 0;

                Hammer.RecordHigh = 0;
                Hammer.RecordLow = 0;
                Hammer.Average = 0;
            }
            //tmp.Reverse();
            //tmp2.Reverse();
        }

        private void LoadExtremes()
        {
            int ListLength = Close.List.Count();
            List<Extreme> extremes = new List<Extreme>();

            for (int step = 0; step + Constants.SweepingWindow < ListLength; step += Constants.SweepingStep)
            {

                List<double> tmpRange = Close.List.GetRange(step, Constants.SweepingWindow);
                double max = tmpRange.Max();
                double min = tmpRange.Min();

                if (tmpRange.IndexOf(max) == Constants.SweepingWindow-1 ||
                    tmpRange.IndexOf(max) == 0 ||
                    tmpRange.Count(x => x == max) > 1)
                {
                    // no good
                    continue;
                }
                else
                {
                    // add to extremes
                    if (extremes.Count(x => IsClose(x.Level, max)) == 0)
                    {
                        Extreme ex = new Extreme();
                        ex.Level = max;
                        ex.Strength = Constants.ChartExtermumInitialWeight;
                        extremes.Add(ex);
                    }
                    else
                    {
                        foreach (Extreme ex in extremes)
                        {
                            if (IsClose(ex.Level, max))
                            {
                                ex.Strength+=Constants.ChartExtermumReapeatedMeetWeightToBeAdded;
                            }
                        }
                    }
                }

                if (tmpRange.IndexOf(min) == Constants.SweepingWindow - 1 ||
                    tmpRange.IndexOf(min) == 0 ||
                    tmpRange.Count(x => x == min) > 1)
                {
                    // no good
                    continue;
                }
                else
                {
                    // add to extremes
                    if (extremes.Count(x => IsClose(x.Level, min)) == 0)
                    {
                        Extreme ex = new Extreme();
                        ex.Level = min;
                        ex.Strength = Constants.ChartExtermumInitialWeight;
                        extremes.Add(ex);
                    }
                    else
                    {
                        foreach (Extreme ex in extremes)
                        {
                            if (IsClose(ex.Level, min))
                            {
                                ex.Strength+=Constants.ChartExtermumReapeatedMeetWeightToBeAdded;
                            }
                        }
                    }
                }

            }

            for (int i = 0; i < extremes.Count - 1; i++)
            {
                // look for crosses to decrease extreme strengths
                int biggerpeaks = extremes.GetRange(i, extremes.Count - i).Count(x => x.Level > extremes[i].Level);
                int smallerpeaks = extremes.GetRange(i, extremes.Count - i).Count(x => x.Level < extremes[i].Level);
                int diff = Math.Abs(biggerpeaks - smallerpeaks);
                extremes[i].Strength -= diff * Constants.ChartExtermumNegligenceMultiplingFactor;
            }


            Extremes= extremes ;
        }

        private bool IsClose(double a, double b)
        {
            double z = a - b;
            double x = z / b;
            double c = Math.Abs(x);

            if (c < Constants.ClosenessThreshold)
                return true;
            else
                return false;
        }

    }
}
