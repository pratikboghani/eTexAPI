using System;
using System.Collections.Generic;
using eTexAPI.Data.Model;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ope = eTexAPI.Data.OperationSql;
using Dapper;
using eTexAPI.Data.Common;

namespace eTexAPI.Data.Services
{
    public class GlbServices
    {
        DapperHelper _dapperHelper = new DapperHelper();
        private DataTable _DT = new DataTable();

        private DataSet _DS = new DataSet();
        private DataSet _DS_VIEW = new DataSet();
        private DataSet _DSWP = new DataSet();

        public DataSet DS { get { return _DS; } }
        public DataSet DS_VIEW { get { return _DS_VIEW; } }
        public DataSet DSWP { get { return _DSWP; } }
        
        public int FindNewMsgLogId()
        {
            return Ope.FindNewId(Ope.EnumServer.mServer, "MessageLog", "MAX(LogID)", "");
        }
        public async Task<int> SaveMsgLog(ClsMsg cFl)
        {
           
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@LogID", cFl.LogID, DbType.Int16);
            parameter.Add("@LogWp", cFl.LogWp, DbType.Int16);
            parameter.Add("@LogMail", cFl.LogMail, DbType.Int16);
            parameter.Add("@LogBrok", cFl.LogBrok, DbType.Int16);
            parameter.Add("@LogParty", cFl.LogParty, DbType.Int16);
            parameter.Add("@FormType", cFl.FormType, DbType.String);
            parameter.Add("@FormKey", cFl.FormKey, DbType.String);
            parameter.Add("@IUser", cFl.IUser, DbType.String);
            
            int x = await _dapperHelper.ExecuteAsync("[dbo].[uSP_MessageLogSave]", parameter, CommandType.StoredProcedure);
            return x;
        }
        
       
    }
}
