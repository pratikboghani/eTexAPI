using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTexAPI.Data.Model
{
    public class clsSaleOrder
    {
        #region Public Properties
        //Primary Key SaleOrderH
        public int OrdNo { get; set; }
        public int DetNo { get; set; }
        public int CCode { get; set; }
        public int YCode { get; set; }


        //Non Primary Key SaleOrderH

        public string ICat { get; set; }
        public DateTime? TrnDate { get; set; }
        public int AC_Code { get; set; }
        public int BAC_Code { get; set; }
        public string ChkName { get; set; }
        public string ChkMobNo { get; set; }
        public int PayTerms { get; set; }
        public string Remark { get; set; }
        public string PartyOrderNo { get; set; }
        public string OrderStatus { get; set; }
        public string IUser { get; set; }

        //Non Primary Key SaleOrderL
        public int ICode { get; set; }
        public int Lot { get; set; }
        public int Pcs { get; set; }
        public int MfgUnit { get; set; }
        public double Meter { get; set; }
        public double Rate { get; set; }
        public string Unit { get; set; }
        public double Amt { get; set; }
        public double IGSTPer { get; set; }
        public double SGSTPer { get; set; }
        public double CGSTPer { get; set; }
        public double IGSTAmt { get; set; }
        public double SGSTAmt { get; set; }
        public double CGSTAmt { get; set; }

        //Non Primary Key View
        public DateTime? F_DATE { get; set; }
        public DateTime? T_DATE { get; set; }
        public string ViewStrStsType { get; set; }

        //End of Properties
        #endregion
    }
}
