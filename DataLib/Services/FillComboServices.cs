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
    public class FillComboServices
    {
        DapperHelper _dapperHelper = new DapperHelper();

        private DataSet _DS = new DataSet();
        private Int16 _OrdFlg = 0;

        /// <summary>
        /// Setting Order Of Record Retrieval 
        /// 0 = Order By Code (By Default First Field)
        /// 1 = Order By Ord Field 
        /// </summary> 
        public Int16 IsOrderBy
        {
            set { _OrdFlg = value; }
        }

        /// <summary>
        /// Retrieve Dataset
        /// </summary>
        public DataSet DS { get { return _DS; } set { _DS = value; } }

        private string _TableName = "";
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        public string ICAT { get; set; }
        public string USER { get; set; }
        public string Typ { get; set; } = "";
        public string GrpCode { get; set; } = "";
        public int C_Code { get; set; } = 0;

        public int Dept_Code { get; set; } = 0;
        public int Y_Code { get; set; } = 0;
        public int MfgUnit { get; set; } = 1;
        public DateTime? Prod_Date { get; set; }
        public int MacNo { get; set; }


        public void Fill()
        {
            Ope.Clear();

            //Ope.AddParams("C_Code", C_Code);
            //Ope.AddParams("Y_Code", Y_Code);
            Ope.AddParams("Type", _TableName);
            Ope.AddParams("ChkOrd", _OrdFlg.ToString());
            Ope.AddParams("TYP", Typ);
            Ope.AddParams("GrpCode", GrpCode);
            Ope.AddParams("ICAT", ICAT);
            Ope.AddParams("Dept_Code", Dept_Code);
            Ope.AddParams("MfgUnit", MfgUnit);
            Ope.AddParams("Prod_Date", Prod_Date);
            Ope.AddParams("MacNo", MacNo);
            Ope.AddParams("USER", USER);
            Ope.FillDataSet(Ope.EnumServer.mServer, DS, _TableName, "SP_Mast_CmbFill", Ope.GetParams());

        }
    }
        
}
