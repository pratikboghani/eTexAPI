using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Transactions;

namespace eTexAPI.Data
{
    public class GlobalSql
    {
        #region 
 
        public static string GConnString = string.Empty;
        
        #endregion

        #region UserDet
        
        public static string gStrDateFormat = "MM/dd/yy";
        public static string gStrServerDate = "MM/dd/yyyy";
        public static string gStrTimeFormat = "HH:MM tt";
         

        public DateTime? gDTServerDate = null;
        #endregion

        #region

        public static SqlConnection GConn;
        public static SqlDataAdapter GDataAdapter = new SqlDataAdapter();
        public static SqlCommand GComm = new SqlCommand();
        public static SqlDataReader GDataReader;
        public static SqlTransaction GSqlTran;

        public static TransactionScope gTransactionScope = new TransactionScope();

        #endregion

        public static string gStrErrMsg = "";

        #region Global Message

        public static Int32 GIntMesgMode;

        public static string GStrMsgCaption = "WorkFlow";

        public static string GStrMsgDeleteDeny = "Sorry, Delete Permission Denied";
        
        public static string GStrMsgInsertDeny = "Sorry, Insert Permission Denied";
                
        public static string GStrMsgUpdateDeny = "You have not Permission For Update";
        
        public static string GStrMsgViewDeny = "Sorry, View Permission Denied";
        
        public static string GStrMsgPerNotSet = "Sorry, Permission Not Set. Contact To System Administrator";

        public static string GStrMsgInsUpdDeny = "Sorry, Insert Or Update Permission Denied";

        public static string GstrMsgDataNotFound = "Data Not Found !!";

        public static string GstrMsgInwardSuccess = "Slip Inward SuccessFull !!";

        public static string GStrMsgHeading = "Diamond";

        public static string GStrEmail_Pc = "";
        public static string GstrEmail_Default = "";
        public static string GStrEmail_SendList = "";

        public static string GStrLatestExePath = "";
        public static DateTime? gDtServerExeModifyDate = null;

        public static bool GBlnFilTErr = false;

        #endregion
    }
}