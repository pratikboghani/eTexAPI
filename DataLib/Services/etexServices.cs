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
    public class etexServices
    {
        DapperHelper _dapperHelper = new DapperHelper();
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
            Ope.AddParams("Type",    cls.TYPE );
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
            Ope.AddParams("FLOOR", cls.FlorNo);
            Ope.FillDataSet(Ope.EnumServer.mServer, DS, TableName, "Api_AttTrnFill", Ope.GetParams());

        }

//        {
//    "mfgunit":1,
//    "Dept_Code":1,
//    "Att_Date":"13-04-2021",
//    "SrNo":1,      //optional
//    "F_MacNo":223,
//    "T_MacNo":232,
//    "Tot_Mac":10,
//    "Shift":"D",
//    "Emp_Code":25,
//    "BEmp_Code":0,
//    "IUser":"ADMIN",
//    "IComp":"STECH3",
//    "FlorNo":"FIRST",
//    "PDAY":1
//}
    public async Task<int> TrnSave(AttTrn cFl)
        {
            
            if (cFl.SrNo==0 )
            {
                cFl.SrNo = FindNewSrNo(cFl.MFGUNIT.ToString(), cFl.DEPT_CODE.ToString(), cFl.Att_Date.ToString(), cFl.FlorNo, cFl.SHIFT);
            }
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@mfgunit", cFl.MFGUNIT, DbType.Int16);
            parameter.Add("@Dept_Code", cFl.DEPT_CODE, DbType.Int16);
            parameter.Add("@Att_Date", cFl.Att_Date, DbType.DateTime);
            parameter.Add("@SrNo", cFl.SrNo,DbType.Int16);
            parameter.Add("@F_MacNo", cFl.F_MacNo, DbType.Int16);
            parameter.Add("@T_MacNo", cFl.T_MacNo, DbType.Int16);
            parameter.Add("@Tot_Mac", cFl.Tot_Mac, DbType.Int16);
            parameter.Add("@Shift", cFl.SHIFT, DbType.String);
            parameter.Add("@Emp_Code", cFl.Emp_Code, DbType.Int16);
            parameter.Add("@BEmp_Code", cFl.BEmp_Code, DbType.Int16);
            parameter.Add("@IUser", cFl.IUser, DbType.String);
            parameter.Add("@IComp", cFl.IComp, DbType.String);
            parameter.Add("@FlorNo", cFl.FlorNo, DbType.String);
            parameter.Add("@PDAY", cFl.PDAY, DbType.Double);
            int x = await _dapperHelper.ExecuteAsync("[dbo].[uSp_AttTrnSave]", parameter, CommandType.StoredProcedure);
            return x;
        }
        public int FindNewSrNo(string pIntMfgUnit, string pStrDeptCode, string pStrDate, string pStrFlorNo, string pStrShift)
        {
            return Ope.FindNewId(Ope.EnumServer.mServer, TableName, "Max(SRNO)", "MfgUnit = " + pIntMfgUnit + " And Dept_Code = '" + pStrDeptCode + "' And Att_Date = '" + Ope.SqlDate(pStrDate.ToString()) + "' And FlorNo = '" + pStrFlorNo + "' And Shift = '" + pStrShift + "'");
        }
    }
}
