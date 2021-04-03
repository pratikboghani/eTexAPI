using Dapper;
using eTexAPI.Data.Common;
using eTexAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ope = eTexAPI.Data.OperationSql;

namespace eTexAPI.Data.Services
{
    public class SettingServices
    {
        private readonly IDapperHelper _dapperHelper;

        public SettingServices()
        {
            _dapperHelper = new DapperHelper();
        }

        public string GetSQLVersion()
        {
            string data = _dapperHelper.Get<string>("Select @@Version",null, commandType: CommandType.Text);
            return data;
        }

        public string GetByID(string pStrSKey)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SKey", pStrSKey, DbType.String);
            return _dapperHelper.Get<string>("Select SVALUE From dbo.SETTINGS Where SKEY=@SKey", param, CommandType.Text);
        }



        public DataTable CmbFill(CmbFill cmbFill)
        {
            DataTable dt = new DataTable();
            Ope.Clear();
            Ope.AddParams("Type", cmbFill.Type);
            Ope.AddParams("ChkOrd", cmbFill.ChkOrd);
            Ope.AddParams("TYP", cmbFill.Typ);
            Ope.AddParams("Dept_Code", cmbFill.Dept_Code);
            Ope.AddParams("MfgUnit",cmbFill.MfgUnit);

            //Ope.AddParams("U_NAME", cmbFill.U_Name);
            //Ope.AddParams("AlsowithyouAdded", cmbFill.AlsoWithYouAdded);
            Ope.FillDataTable(Ope.EnumServer.mServer, dt, "SP_Mast_CmbFill", Ope.GetParams());
            return dt;
        }

    }
}
