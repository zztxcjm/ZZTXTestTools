using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    class TaskIdMaps
    {


        private static Dictionary<string, long> _urlToTaskIdMaps = new Dictionary<string, long>();
        private static object _lock1 = new object();
        private static object _lock2 = new object();

        public static void Set(string url, long taskId)
        {

            lock (_lock1)
            {
                if (_urlToTaskIdMaps.ContainsKey(url))
                    _urlToTaskIdMaps[url] = taskId;
                else
                    _urlToTaskIdMaps.Add(url, taskId);
            }

        }

        public static long? GetId(string url)
        {
            if (_urlToTaskIdMaps.ContainsKey(url))
                return _urlToTaskIdMaps[url];
            else
                return null;
        }

        public static void RemoveId(string url)
        {

            lock (_lock2)
            {
                if (_urlToTaskIdMaps.ContainsKey(url))
                    _urlToTaskIdMaps.Remove(url);
            }
        }

    }
}
