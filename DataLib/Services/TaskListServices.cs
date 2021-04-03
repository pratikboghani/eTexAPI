using Dapper;
using eTexAPI.Data.Common;
using eTexAPI.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Ope = eTexAPI.Data.OperationSql;
namespace eTexAPI.Data.Services
{
    public class TaskListServices
    {
        #region Var
        private DataSet _DS = new DataSet();
        public DataSet DS
        {
            get { return _DS; }
        }
        public string TableName = "TaskList";

        public enum DetTyp
        {
            Det,
            Sum
        }
        #endregion 

        private readonly IDapperHelper _dapperHelper;

        public TaskListServices()
        {
            _dapperHelper = new DapperHelper();
        }

        public void TaskList(string pStrUserName, string pStrCCode)
        {
            Ope.Clear();
            Ope.AddParams("U_Name", pStrUserName);
            Ope.AddParams("Typ", "PEN");
            Ope.AddParams("AlsowithyouAdded", "TRUE");
            Ope.AddParams("DetTyp", DetTyp.Det.ToString());
            Ope.AddParams("CCode", pStrCCode);
            Ope.FillDataSet(Ope.EnumServer.mServer, DS, TableName, "Api_TaskListCount", Ope.GetParams());            
        }

        public IEnumerable<TaskList> GetByID(string pStrSerKey)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SerKey", pStrSerKey, DbType.String);
            //List<TaskList> data = _dapperHelper.GetAll<TaskList>("Select * From LicTrn Where SerKey=@SerKey", new[] {
            //                            new SqlParameter("@OrganizationId", OrganizationId),
            //                            new SqlParameter("@CompanyId", filter.CompanyId),
            //}, commandType: CommandType.Text);
            return null;
        }

        //public bool SaveTask(TaskMastTrn cTask)
        //{
        //    using (TransactionScope transactionScope = new TransactionScope())
        //    {
        //        DynamicParameters parameter = new DynamicParameters();
        //        parameter.Add("@CCode", cTask.CCode, DbType.String);
        //        parameter.Add("@TaskID", cTask.TaskID, DbType.Int64);
        //        parameter.Add("@Product", cTask.Product, DbType.String);
        //        parameter.Add("@PCName", cTask.PCName, DbType.String);
        //        parameter.Add("@AssignTo", cTask.AssignTo, DbType.String);
        //        parameter.Add("@Task_Title", cTask.Task_Title, DbType.String);
        //        parameter.Add("@Task_desc", cTask.Task_Desc, DbType.String);
                
        //        _dapperHelper.Execute("[dbo].[usp_TaskMastSave]", parameter, commandType: CommandType.StoredProcedure);
        //        transactionScope.Complete();
        //        return true;
        //    }
        //}
         
    }
}
