using StocksHelper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public class Market
    {
        public List<Category> Categories { get; set; }
        public Stock MarketIndex { get; set; }
        public Stock IndustryIndex { get; set; }
        public Stock Big30Index { get; set; }
        public Stock Big50Index { get; set; }

        public Market()
        {
            Categories = new List<Category>();
            MarketIndex = new Stock();
            IndustryIndex = new Stock();
            Big30Index = new Stock();
            Big50Index = new Stock();
        }


        public void Init()
        {
            // calculate data for each stock and category

        }

        public void ApplyCriteria(List<Criterion> lst, DateTime dt)
        {
            foreach (Category cat in Categories)
            {
                cat.CategoryIndex.PassedCriteria.Clear();
                foreach (Stock stk in cat.Stocks)
                {
                    stk.PassedCriteria.Clear();
                }
            }

            foreach (Category cat in Categories)
            {
                foreach(Stock stk in cat.Stocks)
                {
                    foreach (DataBundle db in stk.DataBundles)
                    {
                        foreach (Criterion cri in lst)
                        {
                            if (db.List.Count() > cri.HeadPeriod + cri.TailPeriod)
                            {
                                List<double> D = GetSubListD(db, dt, cri.HeadPeriod + cri.TailPeriod);
                                List<double> DD = GetSubListDD(D);
                                if (D.Count == 0 || DD.Count == 0)
                                    continue;
                                if (CriterionAnalyser.AnalyseDerivative(D, cri, db) && CriterionAnalyser.AnalyseDerivativeDerivative(DD, cri, db) && CriterionAnalyser.AnalyseAmount(GetSubListA(db, dt, cri.HeadPeriod + cri.TailPeriod), cri, db) && CriterionAnalyser.AnalyseSR(GetSubListA(stk.Close, dt, cri.HeadPeriod + cri.TailPeriod),cri,stk.Extremes) && db.Dates.Last() == MarketIndex.DataBundles[0].Dates.Last())
                                {
                                    stk.PassedCriteria.Add(new Property() { Name = db.Name, Criterion = cri });
                                }
                                else if(db.Dates.Last() != MarketIndex.DataBundles[0].Dates.Last())
                                {
                                    //Console.WriteLine(stk.Name);
                                }
                            }
                        }
                    }
                }
            }

            foreach (Category cat in Categories)
            {

                foreach (DataBundle db in cat.CategoryIndex.DataBundles)
                {
                    foreach (Criterion cri in lst)
                    {
                        if (db.List.Count() > cri.HeadPeriod + cri.TailPeriod)
                        {
                            List<double> jj = GetSubListD(db, dt, cri.HeadPeriod + cri.TailPeriod);
                            if (CriterionAnalyser.AnalyseDerivative(jj, cri, db)  && CriterionAnalyser.AnalyseAmount(GetSubListA(db, dt, cri.HeadPeriod + cri.TailPeriod), cri, db) && db.Dates.Last() == MarketIndex.DataBundles[0].Dates.Last())
                            {
                                cat.CategoryIndex.PassedCriteria.Add(new Property() { Name = db.Name, Criterion = cri });
                            }
                        }
                    }
                }
                
            }
        }

        public List<double> GetSubListDD(List<double> dd)
        {
            List<double> tmpListD = new List<double>();

            try
            {
                for (int i = 1; i <= dd.Count - 1; i++)
                {
                    tmpListD.Add(CalcD(dd[i - 1], dd[i], 4));
                }
            }
            catch (Exception ex)
            {

            }

            return tmpListD;

        }

        public List<double> GetSubListD(DataBundle db, DateTime dt, int period)
        {
            List<double> tmpList = new List<double>();
            List<double> tmpListD = new List<double>();

            if (period >= db.List.Count)
                period = db.List.Count-1;
            try
            {
                int indexOfDate = db.Dates.IndexOf(dt);
                if (indexOfDate < 0)
                    indexOfDate = db.List.Count - 1;

                tmpList = db.List.GetRange(indexOfDate - period, period + 1);

                for (int i = 1; i <= tmpList.Count - 1; i++)
                {
                    tmpListD.Add(CalcD(tmpList[i - 1], tmpList[i], 4));
                }
            }
            catch(Exception ex)
            {

            }

            return tmpListD;

        }

        public double CalcD(double first_value, double second_value, int round_position)
        {
            if (first_value == 0 || second_value == 0)
                return 0;
            else
                return (Math.Round((second_value - first_value) / Math.Abs(first_value), round_position));
        }


        public List<double> GetSubListA(DataBundle db, DateTime dt, int period)
        {

            int indexOfDate = db.Dates.IndexOf(dt);
            if (indexOfDate < 0)
                indexOfDate = db.List.Count - 1;
            List<double> tmpList = new List<double>();
            tmpList = db.List.GetRange(indexOfDate - period+1, period);


            return tmpList;

        }

    }
}
