using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    public class FileTaskInfo
    {

        /// <summary>
        /// 文件ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string Md5 { get; set; }

        /// <summary>
        /// 文件完整路径
        /// </summary>
        public string FileFullPath { get; set; }

        /// <summary>
        /// 文件是否被处理过
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 文件鉴定结果
        /// </summary>
        public AppraiseResult Result { get; set; }

        /// <summary>
        /// 文件置信度
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// 图片为性感图片的评分
        /// </summary>
        public double Hot_score { get; set; }

        /// <summary>
        /// 图片为正常图片的评分
        /// </summary>
        public double Normal_score { get; set; }

        /// <summary>
        /// 图片为色情图片的评分
        /// </summary>
        public double Porn_score { get; set; }

        /// <summary>
        /// 文件上传时间
        /// </summary>
        public DateTime FileUploadTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public long? CorpId { get; set; }

    }

    public enum AppraiseResult
    {

        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 色情
        /// </summary>
        Pornographic = 1,

        /// <summary>
        /// 疑似色情
        /// </summary>
        Sexy = 2,

        /// <summary>
        /// 未知
        /// </summary>
        Unknow = -1

    }

}
