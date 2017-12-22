using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoad
{
    class ConcurrentMethodOption
    {

        public ConcurrentMethod Method;

        public ConcurrentMethodOption(ConcurrentMethod m)
        {
            Method = m;
        }

        public override string ToString()
        {
            switch (Method)
            {
                case ConcurrentMethod.Meanwhile:
                    return "同时触发";
                case ConcurrentMethod.Order:
                    return "按顺序递增";
                case ConcurrentMethod.Random:
                    return "时间范围内随机并发";
            }

            return "未知";
        }
    }
}
