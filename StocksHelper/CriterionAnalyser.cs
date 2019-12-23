using StocksHelper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public static class CriterionAnalyser
    {
        public static bool AnalyseDerivative(List<double> lst, Criterion cri, DataBundle db)
        {
            try
            {
                List<double> headlist = lst.GetRange(lst.Count() - cri.HeadPeriod, cri.HeadPeriod);
                List<double> taillist = lst.GetRange(0, cri.TailPeriod);

                if (db.Average == 0)
                    return false;

                ////// head list within range?
                //if (!IsWithinRange(headlist, cri.HeadRangeMin, cri.HeadRangeMax))
                //{
                //    return false;
                //}
                //// tail list within range?
                //if (!IsWithinRange(taillist, cri.TailRangeMin, cri.TailRangeMax))
                //{
                //    return false;
                //}

                // head list violates tolerance?
                if (!PassedTolerance(headlist, 0, 0, cri.HeadRangeMin , cri.HeadRangeMax))
                {
                    return false;
                }

                // tail list violates tolerance?
                if (!PassedTolerance(taillist, cri.TailPullBackTolerance, cri.TailPullbackDistribution, cri.TailRangeMin , cri.TailRangeMax))
                {
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }


        public static bool AnalyseDerivativeDerivative(List<double> lst, Criterion cri, DataBundle db)
        {
            //try
            //{
                List<double> headlist = lst.GetRange(lst.Count() - cri.HeadPeriod, cri.HeadPeriod);
                List<double> taillist = lst.GetRange(0, cri.TailPeriod-1);

                if (db.Average == 0)
                    return false;

                if (cri.HeadSlope == Slope.Positive)
                {
                    var result = headlist.Count(x => x < 0);

                    if (result > 0)
                        return false;
                    return true;
                }
                else if (cri.HeadSlope == Slope.Negative)
                {
                    var result = headlist.Count(x => x > 0);

                    if (result > 0)
                        return false;
                    return true;
                }

                if (cri.TailSlope == Slope.Positive)
                {
                    var result = headlist.Count(x => x < 0);

                    if (result > 0)
                        return false;
                    return true;
                }
                else if (cri.TailSlope == Slope.Negative)
                {
                    var result = headlist.Count(x => x > 0);

                    if (result > 0)
                        return false;
                    return true;
                }

                return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

        }

        public static bool AnalyseAmount(List<double> lst, Criterion cri, DataBundle db)
        {
            List<double> headlist = lst.GetRange(lst.Count() - cri.HeadPeriod, cri.HeadPeriod);
            List<double> taillist = lst.GetRange(0, cri.TailPeriod);

            if (db.Average == 0)
                return false;

            switch (cri.HeadAverageAmount)
            {
                case Enum.Amount.N:
                    break;
                case Enum.Amount.BA:
                    if (headlist.Average() > db.Average)
                        return false;
                    break;
                case Enum.Amount.CTRH:
                    if (PositionPercentage(headlist.Average(), db.RecordHigh) < 90)
                        return false;
                    break;
                case Enum.Amount.CTRL:
                    if (PositionPercentage(headlist.Average(), db.RecordHigh) > 10)
                        return false;
                    break;
                case Enum.Amount.OOF:
                    if (PositionPercentage(headlist.Average(), db.RecordHigh) < 20)
                        return false;
                    break;
                case Enum.Amount.OTF:
                    if (PositionPercentage(headlist.Average(), db.RecordHigh) < 40)
                        return false;
                    break;
                case Enum.Amount.OTRF:
                    if (PositionPercentage(headlist.Average(), db.RecordHigh) < 60)
                        return false;
                    break;
                case Enum.Amount.OFF:
                    if (PositionPercentage(headlist.Average(), db.RecordHigh) < 80)
                        return false;
                    break;
                case Enum.Amount.OA:
                    if (headlist.Average() < db.Average)
                        return false;
                    break;
                case Enum.Amount.AA:
                    if (headlist.Average() < db.Average+(db.Average*0.1) && headlist.Average() > db.Average - (db.Average * 0.1))
                        return false;
                    break;
            }

            if (taillist.Count() > 0)
                switch (cri.TailAverageAmount)
                {
                    case Enum.Amount.N:
                        break;
                    case Enum.Amount.BA:
                        if (taillist.Average() > db.Average)
                            return false;
                        break;
                    case Enum.Amount.CTRH:
                        if (PositionPercentage(taillist.Average(), db.RecordHigh) < 90)
                            return false;
                        break;
                    case Enum.Amount.CTRL:
                        if (PositionPercentage(taillist.Average(), db.RecordHigh) > 10)
                            return false;
                        break;
                    case Enum.Amount.OOF:
                        if (PositionPercentage(taillist.Average(), db.RecordHigh) < 20)
                            return false;
                        break;
                    case Enum.Amount.OTF:
                        if (PositionPercentage(taillist.Average(), db.RecordHigh) < 40)
                            return false;
                        break;
                    case Enum.Amount.OTRF:
                        if (PositionPercentage(taillist.Average(), db.RecordHigh) < 60)
                            return false;
                        break;
                    case Enum.Amount.OFF:
                        if (PositionPercentage(taillist.Average(), db.RecordHigh) < 80)
                            return false;
                        break;
                    case Enum.Amount.OA:
                        if (taillist.Average() < db.Average)
                            return false;
                        break;
                    case Enum.Amount.AA:
                        if (taillist.Average() < db.Average + (db.Average * 0.1) && taillist.Average() > db.Average - (db.Average * 0.1))
                            return false;
                        break;
                }

            return true;

        }


        public static bool AnalyseSR(List<double> lst, Criterion cri, List<Extreme> extremes)
        {
            List<double> headlist = lst.GetRange(lst.Count() - cri.HeadPeriod, cri.HeadPeriod);
            List<double> taillist = lst.GetRange(0, cri.TailPeriod);

            double before = -99999;
            double next = 99999;

            foreach (Extreme ex in extremes)
            {
                if (ex.Level > headlist.Last() && ex.Level < next)
                {
                    next = ex.Level;
                }
                if (ex.Level < headlist.Last() && ex.Level > before)
                {
                    before = ex.Level;
                }
            }

            double canal_width = Math.Round(((next - before) / before) * 100, 2);
            double position = Math.Round(((headlist.Last() - before) / (next - before)) * 100, 2);

            if (next == 99999)
                return true;

            if (before == -99999 && cri.HeadSRPosition != Enum.SRPosition.N)
                return false;


            switch (cri.HeadSRPosition)
            {
                case Enum.SRPosition.N:
                    return true;
                case Enum.SRPosition.U5:
                    if (position > 5)
                        return false;
                    break;
                case Enum.SRPosition.U10:
                    if (position > 10)
                        return false;
                    break;
                case Enum.SRPosition.U15:
                    if (position > 15)
                        return false;
                    break;
                case Enum.SRPosition.U20:
                    if (position > 20)
                        return false;
                    break;
                case Enum.SRPosition.U25:
                    if (position > 25)
                        return false;
                    break;
                case Enum.SRPosition.U30:
                    if (position > 30)
                        return false;
                    break;
                case Enum.SRPosition.U40:
                    if (position > 40)
                        return false;
                    break;
                case Enum.SRPosition.U50:
                    if (position > 50)
                        return false;
                    break;
                case Enum.SRPosition.U60:
                    if (position > 60)
                        return false;
                    break;
                case Enum.SRPosition.U70:
                    if (position > 70)
                        return false;
                    break;
                case Enum.SRPosition.U80:
                    if (position > 80)
                        return false;
                    break;
                case Enum.SRPosition.U90:
                    if (position > 90)
                        return false;
                    break;
                default:
                    break;
            }



            return true;
        }

        private static double PositionPercentage(double v, double max)
        {
            return (v / max) * 100;
        }

        private static bool IsWithinRange(List<double> lst, double min, double max)
        {
            if (min >= 0)
            {
                int cnt = lst.Count(x => x > 0 && (x < min || x > max));
                if (cnt > 0 || lst.Count(x => x==0) > 0)
                    return false;
            }
            if (max <= 0)
            {
                int cnt = lst.Count(x => x < 0 && (x < min || x > max));
                if (cnt > 0 || lst.Count(x => x == 0) > 0)
                    return false;
            }
            return true;
        }

        private static bool PassedTolerance(List<double> lst, double tol, double distri , double min , double max)
        {
            int opposits = 0;
            if (min >= 0)
            {
                opposits = lst.Count(x => x < 0);
            }
            else if (max <= 0)
            {
                opposits = lst.Count(x => x > 0);
            }
            else
            {
                opposits = 0;
            }

            double z = (double)opposits / (double)lst.Count();
            if (z > tol)
                return false;
            else
            {
                // passed tolerance, see if it passes distribution too
                int allowedConsecutiveNumber = Convert.ToInt32(Math.Floor(lst.Count()*tol*distri));   // e.g. 3 consecutive negatives

                if (min >= 0)
                {
                    int count = 0;
                    var result = lst
                            .GroupBy(n => n > 0 ? ++count : count)
                            .Select(x => x.Count(n => n < 0));

                    if (result.Count(x => x> allowedConsecutiveNumber) > 0)
                        return false;
                    return true;
                }
                else
                {
                    int count = 0;
                    var result = lst
                            .GroupBy(n => n < 0 ? ++count : count)
                            .Select(x => x.Count(n => n > 0));

                    if (result.Count(x => x > allowedConsecutiveNumber) > 0)
                        return false;
                    return true;
                }
            }
        }

        private static bool PassesDistribution(List<double> lst, double distri, double min, double max)
        {
            int opposits = 0;
            if (min >= 0)
            {
                opposits = lst.Count(x => x < 0);
            }
            else if (max <= 0)
            {
                opposits = lst.Count(x => x > 0);
            }
            else
            {
                opposits = 0;
            }


            return true;
        }
    }
}
