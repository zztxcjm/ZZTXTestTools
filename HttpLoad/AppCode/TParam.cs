using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoad
{
    public class TParam
    {
        public int Index;
        public int TimeOut=30;
        public string Url;
        public string Agent = "ZZTX_Http_Load_1.0";
        public string Httpmethod = "GET";
        public string Postdata = string.Empty;
        public Dictionary<string, string> Httpheader;
        public ConcurrentMethod ConcurrentMethod;
        public string[] Resultcache;
        public double[] Responsetime;

        public int RandomTimeRange = 30;
        public int OrderInterval = 5;
        public int OrderIncrease = 20;

        public int Sleep = 0;

        public Percent PercentData;

        public TParam Copy()
        {
            return this.MemberwiseClone() as TParam;
        }
    }
}
