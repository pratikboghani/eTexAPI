using System;
using System.Collections.Generic;
using eTexAPI.Data.Model;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ope = eTexAPI.Data.OperationSql;

namespace eTexAPI.Data.Services
{
    public class etexServices
    {
        private DataSet _DS = new DataSet();
        public DataSet DS
        {
            get { return _DS; }
        }
        public string TableName = "AttTrn";


        public void AttCmbDisp(AttTrn cls)
        {
            DataSet dataSet = new DataSet();
            //DynamicParameters parameter = new DynamicParameters();
            Ope.Clear();
            Ope.AddParams("MfgUnit",      cls.MFGUNIT   );
            Ope.AddParams("Dept_Code",    cls.DEPT_CODE );
            //Ope.AddParams("@Att_Date",  cls.Att_Date  );
            //Ope.AddParams("@SHIFT",     cls.SHIFT     );
            //Ope.AddParams("@FLOOR",     cls.FLOOR     );

            //var data = _dapperHelper.GetAll<FileTrn>("[dbo].[uSp_DeptMastFill]", parameter, CommandType.StoredProcedure);
            //var data = _dapperHelper.GetAll<AttTrn>("[dbo].[uSp_DeptMastFill]", parameter, CommandType.StoredProcedure);
            Ope.FillDataSet(Ope.EnumServer.mServer, DS, TableName, "Api_CmbFill", Ope.GetParams());
        }
        public void AttTrnFill(AttTrn cls)
        {
            Ope.Clear();
            Ope.AddParams("MFGUNIT",   cls.MFGUNIT   );
            Ope.AddParams("DEPT_CODE", cls.DEPT_CODE );
            Ope.AddParams("ATT_DATE",  cls.Att_Date  );
            Ope.AddParams("SHIFT",     cls.SHIFT     );
            Ope.AddParams("FLOOR", cls.FLOOR);
            Ope.FillDataSet(Ope.EnumServer.mServer, DS, TableName, "Api_AttTrnFill", Ope.GetParams());

        }
    }
}
