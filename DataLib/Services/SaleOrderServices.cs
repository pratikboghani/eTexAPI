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
    public class SaleOrderServices
    {
        DapperHelper _dapperHelper = new DapperHelper();
        private DataTable _DT = new DataTable();

        private DataSet _DS = new DataSet();
        private DataSet _DS_VIEW = new DataSet();
        private DataSet _DSWP = new DataSet();

        public DataSet DS { get { return _DS; } }
        public DataSet DS_VIEW { get { return _DS_VIEW; } }
        public DataSet DSWP { get { return _DSWP; } }

        public string TableName = "SaleOrderH";


        public void SaleOrderView(clsSaleOrder cls)
        {
            Ope.Clear();
            Ope.AddParams("MfgUnit", cls.MfgUnit);
            Ope.AddParams("YCode", cls.YCode);
            Ope.AddParams("StrStsType", cls.ViewStrStsType);
            Ope.AddParams("FDate", cls.F_DATE);
            Ope.AddParams("TDate", cls.T_DATE);
            Ope.FillDataSet(Ope.EnumServer.mServer, DS_VIEW, TableName, "uSP_SaleOrdView", Ope.GetParams());
        }
        public int GetNewOrdNo(int mfg,int ycode)
        {
            return Ope.FindNewId(Ope.EnumServer.mServer, TableName, "MAX(OrdNo)", " MfgUnit=" + mfg + " And YCode=" + ycode+ "");
        }
       
        public async Task<int> HSave(clsSaleOrder cFl)
        {
           
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@MfgUnit", cFl.MfgUnit, DbType.Int16);
            parameter.Add("@YCode", cFl.YCode, DbType.Int16);
            parameter.Add("@OrdNo", cFl.OrdNo, DbType.Int16);
            parameter.Add("@ICat", cFl.ICat,DbType.String);
            parameter.Add("@TrnDate", cFl.TrnDate, DbType.DateTime);
            parameter.Add("@AC_Code", cFl.AC_Code, DbType.Int16);
            parameter.Add("@BAC_Code", cFl.BAC_Code, DbType.Int16);
            parameter.Add("@ChkName", cFl.ChkName, DbType.String);
            parameter.Add("@ChkMobNo", cFl.ChkMobNo, DbType.String);
            parameter.Add("@PayTerms", cFl.PayTerms, DbType.Int16);
            parameter.Add("@Remark", cFl.Remark, DbType.String);
            parameter.Add("@PartyOrderNo", cFl.PartyOrderNo, DbType.String);
            parameter.Add("@OrderStatus", cFl.OrderStatus, DbType.String);
            parameter.Add("@IUser", cFl.IUser, DbType.String);
            
            int x = await _dapperHelper.ExecuteAsync("[dbo].[uSP_SaleHSave]", parameter, CommandType.StoredProcedure);
            return x;
        }
        public async Task<int> LSave(clsSaleOrder cFl)
        {
            
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@MfgUnit", cFl.MfgUnit, DbType.Int16);
            parameter.Add("@YCode", cFl.YCode, DbType.Int16);
            parameter.Add("@OrdNo", cFl.OrdNo, DbType.Int16);
            parameter.Add("@DetNo", cFl.DetNo,DbType.Int16);
            parameter.Add("@ICode", cFl.ICode, DbType.Int16);
            parameter.Add("@Lot", cFl.Lot, DbType.Int16);
            parameter.Add("@Pcs", cFl.Pcs, DbType.Int16);
            parameter.Add("@Meter", cFl.Meter, DbType.Double);
            parameter.Add("@Rate", cFl.Rate, DbType.Int32);
            parameter.Add("@Unit", cFl.Unit, DbType.String);
            parameter.Add("@Amt", cFl.Amt, DbType.Double);
            parameter.Add("@IGSTPer", cFl.IGSTPer, DbType.Double);
            parameter.Add("@SGSTPer", cFl.SGSTPer, DbType.Double);
            parameter.Add("@CGSTPer", cFl.CGSTPer, DbType.Double);
            parameter.Add("@IGSTAmt", cFl.IGSTAmt, DbType.Double);
            parameter.Add("@SGSTAmt", cFl.SGSTAmt, DbType.Double);
            parameter.Add("@CGSTAmt", cFl.CGSTAmt, DbType.Double);
            
            int x = await _dapperHelper.ExecuteAsync("[dbo].[uSP_SaleLSave]", parameter, CommandType.StoredProcedure);
            return x;
        }
       
    }
}
