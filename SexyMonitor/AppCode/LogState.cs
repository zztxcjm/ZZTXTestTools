using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    class LogState
    {

        public static bool Enabled
        {
            get
            {
                var str = System.Configuration.ConfigurationManager.AppSettings["WriteBizLog"];
                if (String.IsNullOrEmpty(str))
                    return true;

                str = str.ToLower();
                return str == "true";

            }
        }
    }
}
