using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper.Enum
{
    public enum Amount
    {
        BA, //BelowAverage,
        OA, //OverAverage,
        AA, //AroundAverage
        OOF,  //OverOneFifthOfRecordHigh,
        OTF, //OverTwoThirdsOfRecordHigh,
        OTRF,  //OverThreeFifthOfRecordHigh,
        OFF, //OverFourThirdsOfRecordHigh,
        CTRH, //CloseToRecordHigh,
        CTRL, //CloseToRecordLow
        N   // no setting
    }
}
