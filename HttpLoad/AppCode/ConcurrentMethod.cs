using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLoad
{
    public enum ConcurrentMethod
    {

        /// <summary>
        /// 同时触发
        /// </summary>
        Meanwhile = 0,
        /// <summary>
        /// 按顺序递增
        /// </summary>
        Order = 1,
        /// <summary>
        /// 时间范围内随机并发
        /// </summary>
        Random = 2


    }
}
