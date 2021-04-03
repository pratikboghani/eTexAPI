using Dapper;
using eTexAPI.Data.Common;
using eTexAPI.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Ope = eTexAPI.Data.OperationSql;
namespace eTexAPI.Data.Services
{
    public class TaskServices
    {
        private readonly IDapperHelper _dapperHelper;

        private DataSet _DS = new DataSet();
        public DataSet DS
        {
            get { return _DS; }
        }
        public string TableName = "AttTrn";

        public enum DetTyp
        {
            Det,
            Sum
        }

        public TaskServices()
        {
            _dapperHelper = new DapperHelper();
        }
        public DataSet AttTrnDisp()
        {
            DataSet dataSet = new DataSet();

            //DynamicParameters parameter = new DynamicParameters();
            Ope.Clear();
            //Ope.AddParams("@MfgUnit",   cls.MFGUNIT   );
            //Ope.AddParams("@Dept_Code", cls.DEPT_CODE );
            //Ope.AddParams("@Att_Date",  cls.Att_Date  );
            //Ope.AddParams("@SHIFT",     cls.SHIFT     );
            //Ope.AddParams("@FLOOR",     cls.FLOOR     );

            //var data = _dapperHelper.GetAll<FileTrn>("[dbo].[uSp_DeptMastFill]", parameter, CommandType.StoredProcedure);
            //var data = _dapperHelper.GetAll<AttTrn>("[dbo].[uSp_DeptMastFill]", parameter, CommandType.StoredProcedure);
            Ope.FillDataSet(Ope.EnumServer.mServer, dataSet, TableName, "uSp_DeptMastFill", Ope.GetParams());
            return dataSet;
        }
        
        //public DataSet MfgUnitMastDisp()
        //{
        //    //DynamicParameters parameter = new DynamicParameters();
        //    //parameter.Add("@CCode", pStrCCode, DbType.String);
        //    //parameter.Add("@TaskID", pIntTaskID, DbType.Int16);

        //    //var data = _dapperHelper.GetAll<FileTrn>("[dbo].[uSp_DeptMastFill]", parameter, CommandType.StoredProcedure);


        //    DataSet dataSet = new DataSet();
        //    Ope.FillDataSet(Ope.EnumServer.mServer, dataSet, TableName, "uSp_DeptMastFill", Ope.GetParams());
        //    return dataSet;
        //}
        //public int SaveTask_O(TaskMastTrn cTask)
        //{
        //    try
        //    {
        //        using (TransactionScope transactionScope = new TransactionScope())
        //        {
        //            DynamicParameters parameter = new DynamicParameters();
        //            parameter.Add("@CCode", cTask.CCode, DbType.String);
        //            parameter.Add("@TaskID", cTask.TaskID, DbType.Int64);
        //            parameter.Add("@Product", cTask.Product, DbType.String);
        //            parameter.Add("@PCName", cTask.PCName, DbType.String);
        //            parameter.Add("@AssignTo", cTask.AssignTo, DbType.String);
        //            parameter.Add("@Task_Title", cTask.Task_Title, DbType.String);
        //            parameter.Add("@Task_desc", cTask.Task_Desc, DbType.String);

        //            int x = _dapperHelper.Execute("[dbo].[usp_TaskMastSave]", parameter, commandType: CommandType.StoredProcedure);
        //            transactionScope.Complete();
        //            return x;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {

        //    }
        //    return 0;
        //}
        //public async Task<int> SaveTask(TaskMastTrn cTask)
        //{
        //    DynamicParameters parameter = new DynamicParameters();
        //    parameter.Add("@CCode", cTask.CCode, DbType.String);
        //    parameter.Add("@TaskID", cTask.TaskID, DbType.Int16);
        //    parameter.Add("@Product", cTask.Product, DbType.String);
        //    parameter.Add("@PCName", cTask.PCName, DbType.String);
        //    parameter.Add("@AssignTo", cTask.AssignTo, DbType.String);
        //    parameter.Add("@Task_Title", cTask.Task_Title, DbType.String);
        //    parameter.Add("@Task_desc", cTask.Task_Desc, DbType.String);
        //    int x = await _dapperHelper.ExecuteAsync("[dbo].[usp_TaskMastSave]", parameter, CommandType.StoredProcedure);
        //    return x;
        //}


        //#region File Trn
        ////File Save
        //public async Task<int> FileSave(FileTrn cFl)
        //{
        //    DynamicParameters parameter = new DynamicParameters();
        //    parameter.Add("@CCode", cFl.CCode, DbType.String);
        //    parameter.Add("@TaskID", cFl.TaskID, DbType.Int16);
        //    parameter.Add("@FtpFileName", cFl.FtpFileName, DbType.String);
        //    parameter.Add("@OrgFileName", cFl.OrgFileName, DbType.String);
        //    int x = await _dapperHelper.ExecuteAsync("[dbo].[usp_FileTrnSave]", parameter, CommandType.StoredProcedure);
        //    return x;
        //}


        //public async Task<int> TasklogSave(TaskLog cTL)
        //{
        //    DynamicParameters parameter = new DynamicParameters();
        //    parameter.Add("@CCode", cTL.CCode, DbType.String);
        //    parameter.Add("@TaskID", cTL.TaskID, DbType.Int16);
        //    parameter.Add("@LogId", cTL.LogId, DbType.Int16);
        //    parameter.Add("@Remark", cTL.Remark, DbType.String);
        //    parameter.Add("@DueDate", cTL.DueDate, DbType.Date);
        //    parameter.Add("@IUser", cTL.IUser, DbType.String);
        //    parameter.Add("@AssignTo", cTL.AssignTo, DbType.String);
        //    parameter.Add("@Typ", cTL.Typ, DbType.String);
        //    parameter.Add("@MarkComplete", cTL.MarkComplete, DbType.Boolean);
        //    parameter.Add("@TaskClose", cTL.TaskClose, DbType.Boolean);
        //    parameter.Add("@Taskstart", cTL.Taskstart, DbType.Boolean);
        //    parameter.Add("@TaskPriority", cTL.TaskPriority, DbType.String);
        //    int x = await _dapperHelper.ExecuteAsync("[dbo].[uSP_TasklogSave]", parameter, CommandType.StoredProcedure);
        //    return x;
        //}

        //public DataTable FileTrnDisp(string pStrCCode, int pIntTaskID)
        //{
        //    DynamicParameters parameter = new DynamicParameters();
        //    parameter.Add("@CCode", pStrCCode, DbType.String);
        //    parameter.Add("@TaskID", pIntTaskID, DbType.Int16);
        //    var data = _dapperHelper.GetDataSet("[dbo].[uSP_FileTrnDisp]", param, CommandType.StoredProcedure);
        //    return data;
        //}

        //public DataSet FileTrnDisp(string pStrCCode, int pIntTaskID) 
        //{
        //    DataSet dataSet = new DataSet();
        //    Ope.Clear();
        //    Ope.AddParams("CCode", pStrCCode);
        //    Ope.AddParams("TaskID", pIntTaskID);
        //    Ope.FillDataSet(Ope.EnumServer.mServer, dataSet, TableName, "uSP_FileTrnDisp", Ope.GetParams());
        //    return dataSet;
        //}


        //#endregion

    }

}

