using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace SexyMonitor
{
    class FileTaskDAL : FaceHand.Common.DataAccessBase
    {
        private FileTaskDAL() { }
        private static readonly FileTaskDAL _instance = new FileTaskDAL();

        public static FileTaskDAL GetInstance()
        {
            return _instance;
        }

        internal DataRow SelectFileTask(string md5)
        {
            var sql = "select * from sm_fileinfo where `Md5`='" + md5 + "'";
            return GetDataRow(sql);

        }
        internal DataRow SelectFileTask(long id)
        {
            var sql = "select * from sm_fileinfo where id=" + id;
            return GetDataRow(sql);

        }

        internal bool IsExistFileTask(string md5)
        {
            var sql = "select count(1) from sm_fileinfo where `Md5`='" + md5 + "'";
            return GetInt(sql) > 0;
        }

        internal void Insert(FileTaskInfo fileTaskInfo)
        {
            var sql = "insert into sm_fileinfo (`Md5`,`FileFullPath`,`CorpId`,`FileUploadTime`) value (@Md55,@FileFullPath,@CorpId,@FileUploadTime)";
            using (DbCommand cmd = DbInstance.GetSqlStringCommand(sql))
            {
                DbInstance.AddInParameter(cmd, "Md55", DbType.String, fileTaskInfo.Md5);
                DbInstance.AddInParameter(cmd, "FileFullPath", DbType.String, fileTaskInfo.FileFullPath);
                DbInstance.AddInParameter(cmd, "FileUploadTime", DbType.DateTime, fileTaskInfo.FileUploadTime);
                DbInstance.AddInParameter(cmd, "CorpId", DbType.Int64, fileTaskInfo.CorpId);

                DbInstance.ExecuteNonQuery(cmd);

            }
        }

        internal DataTable SelectFileTaskWaitPorcessing(int num)
        {
            var sql = "select * from sm_fileinfo where IsProcessed=0 and IsProcessing=0 limit 0," + num;
            return GetDataTable(sql);
        }

        internal DataTable SelectFileTaskWhereCorpIdIsNull(int num)
        {
            var sql = "select * from sm_fileinfo where CorpId is null limit 0," + num;
            return GetDataTable(sql);
        }

        internal void Delete(string md5)
        {
            if (!String.IsNullOrEmpty(md5))
            {
                var sql = "delete from sm_fileinfo where `Md5`='" + md5 + "'";
                ExecSql(sql);
            }
        }

        internal void Delete(long id)
        {
            var sql = "delete from sm_fileinfo where id=" + id;
            ExecSql(sql);
        }

        internal void UpdateTaskProcessState(IEnumerable<long> taskIds)
        {
            if (taskIds != null && taskIds.Count() > 0)
            {
                var sql = "update sm_fileinfo set IsProcessing=1 where Id in(" + String.Join(",", taskIds) + ")";
                ExecSql(sql);
            }
        }

        internal void UpdateTaskProcessState2(string md5, bool isProcessed, bool isProcessing, int resultCode, string resultDescription)
        {

            if (String.IsNullOrEmpty(md5))
                return;

            var sql = @"
                        update sm_fileinfo set 
	                        ResultCode=@ResultCode,
	                        ResultDescription=@ResultDescription,
	                        LastProcessTime=@LastProcessTime,
	                        ProcessCount=ProcessCount+1,
	                        IsProcessed=@IsProcessed,
	                        IsProcessing=@IsProcessing
	
	                        where `Md5`=@Md55";

            using (DbCommand cmd = DbInstance.GetSqlStringCommand(sql))
            {
                DbInstance.AddInParameter(cmd, "ResultCode", DbType.Int64, resultCode);
                DbInstance.AddInParameter(cmd, "ResultDescription", DbType.String, resultDescription);
                DbInstance.AddInParameter(cmd, "LastProcessTime", DbType.DateTime, DateTime.Now);
                DbInstance.AddInParameter(cmd, "IsProcessed", DbType.Boolean, isProcessed);
                DbInstance.AddInParameter(cmd, "IsProcessing", DbType.Boolean, isProcessing);
                DbInstance.AddInParameter(cmd, "Md55", DbType.String, md5);

                DbInstance.ExecuteNonQuery(cmd);

            }


        }
        internal void UpdateTaskProcessState3(
            string md5, bool isProcessed, bool isProcessing, int resultCode, string resultDescription,
            AppraiseResult result, double confidence, double hot_score, double normal_score, double porn_score
            )
        {

            if (String.IsNullOrEmpty(md5))
                return;

            var sql = @"
                        update sm_fileinfo set 
	                        ResultCode=@ResultCode,
	                        ResultDescription=@ResultDescription,
	                        LastProcessTime=@LastProcessTime,
	                        ProcessCount=ProcessCount+1,
	                        IsProcessed=@IsProcessed,
	                        IsProcessing=@IsProcessing,
	
	                        Result=@Result,
	                        Confidence=@Confidence,
	                        Hot_score=@Hot_score,
	                        Normal_score=@Normal_score,
	                        Porn_score=@Porn_score	
	
	                        where `Md5`=@Md55";

            using (DbCommand cmd = DbInstance.GetSqlStringCommand(sql))
            {

                DbInstance.AddInParameter(cmd, "ResultCode", DbType.Int64, resultCode);
                DbInstance.AddInParameter(cmd, "ResultDescription", DbType.String, resultDescription);
                DbInstance.AddInParameter(cmd, "LastProcessTime", DbType.DateTime, DateTime.Now);
                DbInstance.AddInParameter(cmd, "IsProcessed", DbType.Boolean, isProcessed);
                DbInstance.AddInParameter(cmd, "IsProcessing", DbType.Boolean, isProcessing);
                DbInstance.AddInParameter(cmd, "Md55", DbType.String, md5);

                DbInstance.AddInParameter(cmd, "Result", DbType.Byte, result);
                DbInstance.AddInParameter(cmd, "Confidence", DbType.Double, confidence);
                DbInstance.AddInParameter(cmd, "Hot_score", DbType.Double, hot_score);
                DbInstance.AddInParameter(cmd, "Normal_score", DbType.Double, normal_score);
                DbInstance.AddInParameter(cmd, "Porn_score", DbType.Double, porn_score);

                DbInstance.ExecuteNonQuery(cmd);

            }


        }


        internal void UpdateTaskProcessState2(long id, bool isProcessed, bool isProcessing, int resultCode, string resultDescription)
        {

            var sql = @"
                        update sm_fileinfo set 
	                        ResultCode=@ResultCode,
	                        ResultDescription=@ResultDescription,
	                        LastProcessTime=@LastProcessTime,
	                        ProcessCount=ProcessCount+1,
	                        IsProcessed=@IsProcessed,
	                        IsProcessing=@IsProcessing
	
	                        where id=@id";

            using (DbCommand cmd = DbInstance.GetSqlStringCommand(sql))
            {
                DbInstance.AddInParameter(cmd, "ResultCode", DbType.Int64, resultCode);
                DbInstance.AddInParameter(cmd, "ResultDescription", DbType.String, resultDescription);
                DbInstance.AddInParameter(cmd, "LastProcessTime", DbType.DateTime, DateTime.Now);
                DbInstance.AddInParameter(cmd, "IsProcessed", DbType.Boolean, isProcessed);
                DbInstance.AddInParameter(cmd, "IsProcessing", DbType.Boolean, isProcessing);
                DbInstance.AddInParameter(cmd, "id", DbType.Int64, id);

                DbInstance.ExecuteNonQuery(cmd);

            }


        }
        internal void UpdateTaskProcessState3(
            long id, bool isProcessed, bool isProcessing, int resultCode, string resultDescription,
            AppraiseResult result, double confidence, double hot_score, double normal_score, double porn_score
            )
        {

            var sql = @"
                        update sm_fileinfo set 
	                        ResultCode=@ResultCode,
	                        ResultDescription=@ResultDescription,
	                        LastProcessTime=@LastProcessTime,
	                        ProcessCount=ProcessCount+1,
	                        IsProcessed=@IsProcessed,
	                        IsProcessing=@IsProcessing,
	
	                        Result=@Result,
	                        Confidence=@Confidence,
	                        Hot_score=@Hot_score,
	                        Normal_score=@Normal_score,
	                        Porn_score=@Porn_score	
	
	                        where id=@id";

            using (DbCommand cmd = DbInstance.GetSqlStringCommand(sql))
            {

                DbInstance.AddInParameter(cmd, "ResultCode", DbType.Int64, resultCode);
                DbInstance.AddInParameter(cmd, "ResultDescription", DbType.String, resultDescription);
                DbInstance.AddInParameter(cmd, "LastProcessTime", DbType.DateTime, DateTime.Now);
                DbInstance.AddInParameter(cmd, "IsProcessed", DbType.Boolean, isProcessed);
                DbInstance.AddInParameter(cmd, "IsProcessing", DbType.Boolean, isProcessing);
                DbInstance.AddInParameter(cmd, "id", DbType.Int64, id);

                DbInstance.AddInParameter(cmd, "Result", DbType.Byte, result);
                DbInstance.AddInParameter(cmd, "Confidence", DbType.Double, confidence);
                DbInstance.AddInParameter(cmd, "Hot_score", DbType.Double, hot_score);
                DbInstance.AddInParameter(cmd, "Normal_score", DbType.Double, normal_score);
                DbInstance.AddInParameter(cmd, "Porn_score", DbType.Double, porn_score);

                DbInstance.ExecuteNonQuery(cmd);

            }


        }

        internal void UpdateTaskProcessState4(List<string> exceptionMd5s)
        {

            if (exceptionMd5s != null && exceptionMd5s.Count() > 0)
            {
                var sql = "update sm_fileinfo set IsProcessed=0,IsProcessing=0 where `Md5` in('" + String.Join("','", exceptionMd5s) + "')";
                ExecSql(sql);
            }
        }

        internal void UpdateTaskProcessState4(List<long> exceptionIds)
        {
            if (exceptionIds != null && exceptionIds.Count() > 0)
            {
                var sql = "update sm_fileinfo set IsProcessed=0,IsProcessing=0 where Id in(" + String.Join(",", exceptionIds) + ")";
                ExecSql(sql);
            }

        }
    }
}