using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageSender.Model
{
    class MovingAverage
    {
        public static double SMA(List<int> data, int period)
        {
            if (period != 0 && data.Count >= period)
            {
                return data.Skip(data.Count - period).Average();
            }
            return 0;
        }
    }
}
