using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoad
{
    public class TParam
    {
        public int index;
        public int timeOut;
        public string url;
        public string agent = "ZZTX_Http_Load_1.0";
        public string httpmethod = "GET";
        public string postdata = string.Empty;
        public Dictionary<string, string> httpheader;
        public ConcurrentMethod concurrentMethod;
        public string[] resultcache;
        public double[] responsetime;

    }
}
