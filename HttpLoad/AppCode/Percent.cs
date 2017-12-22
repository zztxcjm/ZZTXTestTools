using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoad
{
    public class Percent
    {
        public int Total { get; set; }

        public int Completed = 0;
        private object _lock = new object();

        public void UpdateCompleted()
        {
            lock (_lock)
            {
                Completed += 1;
            }
        }

    }
}
