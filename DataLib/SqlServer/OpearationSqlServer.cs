using System;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using GSql = eTexAPI.Data.GlobalSql;
using GOpeSql = eTexAPI.Data.OperationSql;
using EncD = DhKaalPs.EncDec.EncDecAlg;
using System.Globalization;

namespace eTexAPI.Data

{
    /// <summary>
    /// Class For DataBase Operation [ Sql Server ]
    /// </summary>
    /// 

    public class OperationSql : eTexAPI.Data.GlobalSql
    {

        /// <summary>
        /// Set Current Culture Info
        /// </summary>
        /// 

        public static System.Globalization.CultureInfo CultureInfoUS = new System.Globalization.CultureInfo("en-US", false);



        #region Enum Declaration...
        /// <summary>
        /// Enum For Setting Cell Activation State in UltraWinGrid
        /// </summary>
        public enum EnumCellActivation
        {
            /// <summary>
            /// Enum Active Only
            /// </summary>
            ActiveOnly = 1,
            /// <summary>
            /// Enum Allow Edit
            /// </summary>
            AllowEdit = 0,
            /// <summary>
            /// Enum Disable
            /// </summary>
            Disable = 2,
            /// <summary>
            /// Enum NoEdit
            /// </summary>
            NoEdit = 3
        }
        /// <summary>
        /// Enum For Setting Database Operation 
        /// </summary>
        public enum EnumOpeState
        {
            /// <summary>
            /// Enum For Select Record [Value = 0]
            /// </summary>
            Select = 0,
            /// <summary>
            /// Enum For Insert Record [Value = 1]
            /// </summary>
            Insert = 1,
            /// <summary>
            /// Enum For Update Record [Value = 2]
            /// </summary>
            Update = 2,
            /// <summary>
            /// Enum For Delete Record [Value = 3]
            /// </summary>
            Delete = 3
        }

        /// <summary>
        /// Enum : List Of Servers
        /// </summary>
        public enum EnumServer
        {
            mServer = 0
        }

        public enum EnumSqlTran
        {
            Start = 0,
            Continue = 1,
            Stop = 2,
            None = 4,
            Default = 9
        }

        #endregion Enum Declaration...

        #region Utilities Like Password Encription,Reading Configuration File ....

        public static bool ReadDatabaseSetting()
        {
            string StrConnString = string.Empty;

            StrConnString = ConfigurationSettings.AppSettings.Get("Server").ToString();
            if (StrConnString.Length == 0)
            {
                GSql.GConnString = "";
                return false;
            }
            else
            {
                GSql.GConnString = StrConnString;
            }

            string strNCorePs = ConfigurationSettings.AppSettings.Get("nCorePS").ToString();
            if (string.IsNullOrWhiteSpace(strNCorePs))
            {
                return false;
            }
            GSql.GConnString += ";Password=" + EncD.DecriptedString(ConfigurationSettings.AppSettings.Get("nCorePS").ToString());
            return true;
        }


        /// <summary>
        /// Method for Encoding Or Decoding Given Password
        /// </summary>
        /// <param name="pStr">PassWord String </param>
        /// <param name="pStrToEncodeOrDecode"> String For Encode Or Decode [E-Encode,D-Decode]</param>
        /// <returns>String</returns>
        public static string ENCODE_DECODE(string pStr, string pStrToEncodeOrDecode)
        {
            int IntPos;
            string StrPass;
            string StrECode;
            string StrDCode;
            char ChrSingle;

            StrECode = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StrDCode = ")(*&^%$#@!";

            for (int IntLen = 1; IntLen <= 52; IntLen++)
            {
                StrDCode = StrDCode + (Char)(IntLen + 160);
            }

            StrPass = "";
            for (int IntCnt = 0; IntCnt <= pStr.Trim().Length - 1; IntCnt++)
            {
                ChrSingle = char.Parse(pStr.Substring(IntCnt, 1));
                if (pStrToEncodeOrDecode == "E")
                {
                    IntPos = StrECode.IndexOf(ChrSingle, 0);
                }
                else
                {
                    IntPos = StrDCode.IndexOf(ChrSingle, 0);
                }
                if (pStrToEncodeOrDecode == "E")
                {
                    StrPass = StrPass + StrDCode.Substring(IntPos, 1);
                }
                else
                {
                    StrPass = StrPass + StrECode.Substring(IntPos, 1);
                }
            }
            return StrPass;
        }

        #endregion

        #region ConnetionManupulation

        /// <summary>
        /// Connection Create As Well As Open It. 
        /// </summary>
        /// <param name="ServerCon">Database Name</param>
        /// <returns>True If Successful Else False</returns>
        public static Boolean OpenConnection(EnumServer ServerCon)
        {
            try
            {
                switch (ServerCon)
                {
                    case EnumServer.mServer:
                        CCon(GSql.GConn);
                        GSql.GConn = new SqlConnection(GSql.GConnString);
                        if (OCon(GSql.GConn) == false) return false;
                        GSql.GComm.Connection = GSql.GConn;
                        //if (GSql.DiamondDBName == "")
                        //{
                        //    System.Data.SqlClient.SqlConnectionStringBuilder SConStrBuilder = new SqlConnectionStringBuilder(GSql.GConnCLvString);
                        //    GSql.DiamondDBName = SConStrBuilder.InitialCatalog;
                        //    GAdo.DiamondDBPass = SConStrBuilder.Password;
                        //}
                        break;
                }
            }
            catch (Exception Ex)
            {   
                gStrErrMsg = Ex.Message.ToString();
                CCon(GSql.GConn);
                return false;
            }
            return true;
        }
         
        public static bool IsConnectionCheck(EnumServer ServerCon)
        {
            bool Result = false;
            switch (ServerCon)
            {
                case EnumServer.mServer:
                    try
                    {
                        CCon(GSql.GConn);
                        GSql.GConn = new SqlConnection(GSql.GConnString);
                        OCon(GSql.GConn);
                        Result = true;
                        CCon(GSql.GConn);
                    }
                    catch (Exception)
                    {
                        CCon(GSql.GConn);
                        Result = false;
                    }
                    break;
            }
            return Result;
        }

        /// <summary>
        /// Hash Table For Assigning Parameter Names And Values
        /// </summary>
        public static Hashtable HTParam = new Hashtable();
        /// <summary>
        ///Variable Use For Hash Table Parameter Count
        /// </summary>
        public static Int32 IntParamCount = 0;
        /// <summary>
        /// Method For Checking Connection State And Return True Or False
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <returns>True Or False</returns>
        public static bool IsCon(SqlConnection pConn)
        {
            if (pConn.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region "Open Connection"
        /// <summary>
        /// Open A Connection Of Sql Server With given Connection
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        public static Boolean OCon(SqlConnection pConn)
        {
            if (pConn.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    pConn.Open();
                }
                catch (Exception Ex1)
                {
                    gStrErrMsg = Ex1.Message.ToString();
                    CCon(pConn);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Open A Connection Of Sql Server With given ConnectionString
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <param name="pStrConn">ConenctionString</param>
        /// <param name="BlnOpenCon">Opening Conenction Flag</param>
        public static void OCon(SqlConnection pConn, string pStrConn, bool BlnOpenCon)
        {
            CCon(pConn);
            if (BlnOpenCon == true)
            {
                pConn.Open();
            }
            if (pConn.State == System.Data.ConnectionState.Closed)
            {
                pConn.Open();
            }
        }
        /// <summary>
        /// Open A Connection Of Sql Server With Given Criteria
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <param name="pStrServer">Server Name</param>
        /// <param name="pStrDBName">DataBase Name</param>
        /// <param name="pStrDBUser">DataBase User Name</param>
        /// <param name="pStrDBPass">DataBase Password</param>
        public static void OCon(SqlConnection pConn, string pStrServer, string pStrDBName, string pStrDBUser, string pStrDBPass)
        {
            CCon(pConn);
            pConn.ConnectionString = "Data Source = " + pStrServer + "; Initial Catalog = " + pStrDBName + "; User Id = " + pStrDBUser + "; Password = " + pStrDBPass + "; Persist Security Info = True;";
            pConn.Open();
        }

        #endregion

        #region Close Connection

        /// <summary>
        /// Close Connetion Of SqlServer
        /// </summary>
        /// <param name="pConn">Name of Connection</param>
        public static void CCon(SqlConnection pConn)
        {
            if (pConn != null)
            {
                if (pConn.State == System.Data.ConnectionState.Open)
                {
                    pConn.Close();
                    pConn.Dispose();
                    pConn = null;
                }
            }
            else
            {
                pConn = null;
            }
        }
        public static void CCon(EnumServer ServerCon)
        {
            if (ServerCon == EnumServer.mServer)
            {
                if (GSql.GConn != null)
                {
                    if (GSql.GConn.State == System.Data.ConnectionState.Open)
                    {
                        GSql.GConn.Close();
                        GSql.GConn.Dispose();
                        GSql.GConn = null;
                    }
                }
                else
                {
                    GSql.GConn = null;
                }
            } 
        }
        public static void CloseConnection(EnumServer ServerCon)
        {
            if (ServerCon == EnumServer.mServer)
            {

                if (GSql.GConn != null)
                {
                    if (GSql.GConn.State == System.Data.ConnectionState.Open)
                    {
                        GSql.GConn.Close();
                        GSql.GConn.Dispose();
                        GSql.GConn = null;
                    }
                }
                else
                {
                    GSql.GConn = null;
                }
            }
        }


        #endregion Close Connection
        #endregion

        #region Record Manupulation


        /// <summary> Generate Primary Keys(DataTable)
        /// Business DataAdapter for Dataset Without Parameter Connetion With Primary Key
        /// </summary>
        /// <param name="pTab">Data Table </param>
        private static string PKGen(DataTable pTab)
        {
            string Str = "";
            foreach (DataColumn DataColumnPrimaryKey in pTab.PrimaryKey)
            {
                switch (DataColumnPrimaryKey.DataType.Name.ToLower())
                {
                    case "string":
                        Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = '" + pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original] + "' ";
                        break;
                    case "double":
                    case "decimal":
                    case "integer":
                    case "int32":
                    case "int64":
                    case "int16":
                        Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = " + pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original];
                        break;
                    case "datetime":
                        if (pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original].GetType().ToString() == "System.DBNull")
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = Null ";
                        }
                        else
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = '" + SqlDate(pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original].ToString()) + "'";
                        }
                        break;
                    case "boolean":

                        if (pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original].GetType().ToString() == "System.DBNull")
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = Null ";
                        }
                        else
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = " + System.Convert.ToInt32(pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original]);
                        }
                        break;
                }
            }
            return Str;
        }


        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pStrVal">Parameter Value</param>
        public static void AddParams(String pStrKey, String pStrVal)
        {
            HTParam.Add(pStrKey, pStrVal);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pIntVal">Parameter Value</param>
        public static void AddParams(String pStrKey, Int16 pIntVal)
        {
            HTParam.Add(pStrKey, pIntVal);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pIntVal">Parameter Value</param>
        public static void AddParams(String pStrKey, Int32 pIntVal)
        {
            HTParam.Add(pStrKey, pIntVal);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pIntVal">Parameter Value</param>
        public static void AddParams(String pStrKey, Int64 pIntVal)
        {
            HTParam.Add(pStrKey, pIntVal);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pDblVal">Parameter Value</param>
        public static void AddParams(String pStrKey, double pDblVal)
        {
            HTParam.Add(pStrKey, pDblVal);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pDecVal">Parameter Value</param>
        public static void AddParams(String pStrKey, decimal pDecVal)
        {
            HTParam.Add(pStrKey, pDecVal);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pImage">Parameter Image</param>
        public static void AddParams(String pStrKey, byte[] pImage)
        {
            HTParam.Add(pStrKey, pImage);
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey"></param>
        /// <param name="pDatTime"></param>
        public static void AddParams(String pStrKey, DateTime? pDatTime)
        {
            if (pDatTime == null || pDatTime.ToString() == "")
            {
                HTParam.Add(pStrKey, System.DBNull.Value);
            }
            else
            {
                if (pDatTime.Value.ToShortDateString() == "01/01/0001" || pDatTime.Value.ToShortDateString() == "01/01/01")
                {
                    HTParam.Add(pStrKey, DTDBTime(pDatTime).Value.ToString("hh:mm tt"));
                }
                else
                {
                    HTParam.Add(pStrKey, DTDBDate(pDatTime).Value.ToString("MM/dd/yyyy"));
                }
            }
            IntParamCount++;
        }

        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pIntVal">Parameter Value</param>
        public static void AddParams(Hashtable Ht)
        {
            HTParam = Ht;
            IntParamCount = Ht.Count;
        }


        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pIntVal">Parameter Value</param>
        public static void AddParams(String pStrKey, Boolean pBlnVal)
        {
            HTParam.Add(pStrKey, pBlnVal);
            IntParamCount++;
        }

        /// <summary> Get ParaMeters From HashTable
        /// Method For Get Parameters From HashTable With Key And Vaue
        /// </summary>
        /// <returns>SqlParameters Colection</returns>
        public static SqlParameter[] GetParams()
        {
            Int16 IntI = 0;

            SqlParameter[] GetPara = new SqlParameter[HTParam.Count];
            try
            {
                foreach (DictionaryEntry DE in HTParam)
                {
                    try
                    {
                        if (DE.Key.ToString().Substring(0, 2) == "@@")
                        {

                            GetPara[IntI] = new SqlParameter();
                            GetPara[IntI].Direction = ParameterDirection.Output;
                            GetPara[IntI].Size = 5;
                            GetPara[IntI].ParameterName = DE.Key.ToString().Substring(1);
                        }
                        else
                        {
                            if (DE.Value.GetType().Name.ToUpper() == "BYTE[]")
                            {
                                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.Image);
                                GetPara[IntI].Value = DE.Value;
                            }
                            else if (DE.Value.GetType().Name == "DBNull")
                            {
                                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.DateTime);
                                GetPara[IntI].Value = System.DBNull.Value;
                            }
                            else
                            {
                                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), DE.Value.ToString());
                            }
                        }
                    }
                    catch
                    {
                    }

                    IntI++;
                }
            }
            catch (Exception e1)
            {
                gStrErrMsg = "GetParams" + Environment.NewLine + e1.Message.ToString();
            }
            Clear();
            return GetPara;
        }

        /// <summary> Clear All The Parameters From HashTable 
        /// 
        /// </summary>
        public static void Clear()
        {
            HTParam.Clear();
        }

        #endregion

        #region Filling Of Data

        #region With Server Connection
        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name,PrimaryKey)
        /// Business DataAdapter for Dataset With Primary Key using Procedure Name
        /// </summary>
        /// <param name="ServerConn">Server Connection Enum </param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        /// <param name="pStrPrimaryKey">Primary Key</param>
        public static void FillDataSet(EnumServer ServerConn, DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList, string pStrPrimaryKey)
        {
            DataColumn[] DataColumnPrimaryKey;
            int CountParam = pParamList.Length;
            OpenConnection(ServerConn);
            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
            }
            if (pStrPrimaryKey != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKey.Split(',');
                DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
            CCon(ServerConn);
        }

        public static void FillDataSet(EnumServer ServerConn, DataSet pDataSet, string pTableName, string pSqlQuery, string pStrPrimaryKey, string A)
        {
            DataColumn[] DataColumnPrimaryKey;
            OpenConnection(ServerConn);
            GSql.GComm.CommandType = CommandType.Text;
            GSql.GComm.CommandText = pSqlQuery;
            GSql.GComm.Connection = GSql.GConn;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;


            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
                //mboErrlog.SaveError(ex.ToString(), "eTexAPI.Data");

            }
            finally
            {
                GSql.GComm.Parameters.Clear();
            }
        }

        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name)
        /// Business DataAdapter for Dataset Without Primary Key using Procedure Name
        /// </summary>
        /// <param name="ServerConn">Server Connection Enum </param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        public static void FillDataSet(EnumServer ServerConn, DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList)
        {
            DataColumn[] DataColumnPrimaryKey;
            int CountParam = pParamList.Length;
            OpenConnection(ServerConn);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    foreach (DataTable Dtab in pDataSet.Tables)
                    {
                        Dtab.Rows.Clear();
                    }
                    //pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }

        }


        /// <summary> Fill Of DataSet with Stored Procesure(PrimaryKey)
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="ServerConn">Server Connection Enum </param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        /// <param name="pStrPrimaryKey">Name of Primary Key</param>
        public static Boolean FillDataSet(EnumServer ServerConn, DataSet pDataSet, string pTableName, string pProcedureName, string pStrPrimaryKey)
        {
            DataColumn[] DataColumnPrimaryKey;
            Boolean BlnProperPrimaryKey = true;

            if (OpenConnection(ServerConn) == false) return false;

            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
                GBlnFilTErr = true;
                return false;
            }
            if (pStrPrimaryKey != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKey.Split(',');
                DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    if (pDataSet.Tables[pTableName].Columns[StrArray[IntCount]] == null)
                    {
                        BlnProperPrimaryKey = false;
                        break;
                    }
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[StrArray[IntCount]];
                }
                if (BlnProperPrimaryKey == false)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                    return false;
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
            return true;
        }

        /// <summary> Fill Of DataSet with Stored Procesure(Without Primary Key )
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of DataTable</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        public static void FillDataSet(EnumServer ServerConn, DataSet pDataSet, string pTableName, string pProcedureName)
        {
            DataColumn[] DataColumnPrimaryKey;
            OpenConnection(ServerConn);

            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
        }

        #endregion


        #region Without Server Connection
        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name,PrimaryKey)
        /// Business DataAdapter for Dataset With Primary Key using Procedure Name
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        /// <param name="pStrPrimaryKey">Primary Key</param>
        public static void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList, string pStrPrimaryKey)
        {
            DataColumn[] DataColumnPrimaryKey;
            int CountParam = pParamList.Length;
            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.Connection = GSql.GConn;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
            }
            if (pStrPrimaryKey != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKey.Split(',');
                DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }




        }
        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name)
        /// Business DataAdapter for Dataset Without Primary Key using Procedure Name
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        public static void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList)
        {
            DataColumn[] DataColumnPrimaryKey;
            int CountParam = pParamList.Length;

            OpenConnection(EnumServer.mServer);
            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.Connection = GSql.GConn;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(EnumServer.mServer);
            }

        }


        /// <summary> Fill Of DataSet with Stored Procesure(PrimaryKey)
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        /// <param name="pStrPrimaryKey">Name of Primary Key</param>
        public static void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName, string pStrPrimaryKey)
        {
            DataColumn[] DataColumnPrimaryKey;

            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.Connection = GSql.GConn;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }
            if (pStrPrimaryKey != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKey.Split(',');
                DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
        }
        /// <summary> Fill Of DataSet with Stored Procesure(Without Primary Key )
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of DataTable</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        public static void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName)
        {
            DataColumn[] DataColumnPrimaryKey;


            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.Connection = GSql.GConn;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataSet.Tables[pTableName] == null))
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataSet, pTableName);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = (ex.ToString());
            }

        }

        #endregion


        /// <summary> Fill Of DataTable With(StoreProcedure)
        /// Business DataAdapter for DataTable With Parameter Connetion
        /// </summary>
        /// <param name="pDataTable">DataTable</param>
        /// <param name="pProcedureName">Store Procedure Name </param>
        /// <param name="pParamList">Parameter List Array</param>
        public static void FillDataTable(EnumServer ServerConn, DataTable pDataTable, String pProcedureName, SqlParameter[] pParamList)
        {
            int CountParam = pParamList.Length;
            OpenConnection(ServerConn);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataTable == null))
                {
                    pDataTable.Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataTable);
            }
            catch (Exception ex)
            {
                gStrErrMsg = ex.ToString();
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
        }
        /// <summary> Fill Of DataTable With Stored Procedure With Primary Key(DataTable,ProcedureName,ParaList,PrimaryKey)
        /// Business DataAdapter for DataTable With Primary Keys
        /// </summary>
        /// <param name="pConn"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pProcedureName"></param>
        /// <param name="pParamList"></param>
        /// <param name="pStrPrimaryKey">Primary Keys</param>
        public static void FillDataTable(EnumServer ServerConn, DataTable pDataTable, String pProcedureName, SqlParameter[] pParamList, string pStrPrimaryKeys)
        {
            int CountParam = pParamList.Length;
            DataColumn[] DataColumnPrimaryKey;
            OpenConnection(ServerConn);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataTable == null))
                {
                    pDataTable.Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataTable);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = ex.ToString();
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }

            if (pStrPrimaryKeys != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKeys.Split(',');
                DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataTable.Columns[IntCount];
                }
                pDataTable.PrimaryKey = DataColumnPrimaryKey;
            }
        }
        /// <summary> Fill Of DataTable With General Query(DataTable,Query)
        /// Business DataAdapter for DataTable With Parameter Connetion
        /// </summary>
        /// <param name="pDataTable">Name of Table Name</param>
        /// <param name="pProcedureName">Stored Procedure Name</param>
        public static void FillDataTable(EnumServer ServerConn, DataTable pDataTable, string pProcedureName)
        {
            DataColumn[] DataColumnPrimaryKey;
            OpenConnection(ServerConn);

            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataTable == null))
                {
                    pDataTable.Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataTable);
            }
            catch (SqlException ex)
            {
                gStrErrMsg = ex.ToString();
            }
            finally
            {
                CCon(ServerConn);
            }

        }
        /// <summary> Fill Of DataTable With General Query(DataTable,Query)
        /// Business DataAdapter for DataTable With Parameter Connetion
        /// </summary>
        /// <param name="pDataTable">Name of Table Name</param>
        /// <param name="pProcedureName">Stored Procedure Name</param>
        /// <param name="pStrPrimaryKey">Primary Keys</param>
        public static void FillDataTable(EnumServer ServerConn, DataTable pDataTable, string pProcedureName, string pStrPrimaryKeys)
        {
            DataColumn[] DataColumnPrimaryKey;
            OpenConnection(ServerConn);

            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.Connection = GSql.GConn;
            GSql.GDataAdapter.SelectCommand = GSql.GComm;
            try
            {
                if (!(pDataTable == null))
                {
                    pDataTable.Rows.Clear();
                }
                GSql.GDataAdapter.Fill(pDataTable);

                if (pStrPrimaryKeys != "")
                {
                    string[] StrArray;
                    StrArray = pStrPrimaryKeys.Split(',');
                    DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                    for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                    {
                        DataColumnPrimaryKey[IntCount] = pDataTable.Columns[IntCount];
                    }
                    pDataTable.PrimaryKey = DataColumnPrimaryKey;
                }
            }
            catch (SqlException ex)
            {
                gStrErrMsg = ex.ToString();
            }
            finally
            {
                CCon(ServerConn);
            }
        }


        /// <summary> Give DataReader With(Store Procedure)
        /// Use To Executer Store Procedure With Parameter List With General Connetion
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>SqlDataReader With Record</returns>
        public static SqlDataReader ExeRed(String pProcedureName, SqlParameter[] pParamList)
        {
            int CountParam = pParamList.Length;

            for (int i = 0; i < CountParam; i++)
            {
                GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.Connection = GSql.GConn;
            try
            {
                return GSql.GComm.ExecuteReader();
            }
            catch (SqlException e)
            {
                return null;
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
            }
        }

        /// <summary> Give DataReader With(,ServerConn,Store Procedure)
        /// Use To Executer Store Procedure With Parameter List With General Connetion
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>SqlDataReader With Record</returns>
        /// 


        public static SqlDataReader ExeRed(EnumServer ServerConn, string pStr)
        {

            OpenConnection(ServerConn);
            GSql.GComm.CommandType = CommandType.Text;
            GSql.GComm.CommandText = pStr;
            GSql.GComm.Connection = GSql.GConn;
            return GSql.GComm.ExecuteReader();

        }

        public static SqlDataReader ExeRed(EnumServer ServerConn, String pProcedureName, SqlParameter[] pParamList)
        {
            OpenConnection(ServerConn);
            int CountParam = pParamList.Length;

            for (int i = 0; i < CountParam; i++)
            {
                GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            try
            {
                return GSql.GComm.ExecuteReader();
            }
            catch (SqlException)
            {
                return null;
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
            }
        }


        /// <summary> Give String With (Store Procedure)
        /// Use To Execute Store Procedure With Parameter List and With Connection As Perameter
        /// </summary>
        /// <param name="pConn">Name of Connetion</param>
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>Number of Affected Record Or If error raise then return -1</returns>
        public static string ExeScal(EnumServer ServerConn, String pProcedureName, SqlParameter[] pParamList)
        {
            String Str = "";
            int CountParam = pParamList.Length;
            OpenConnection(ServerConn);
            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GSql.GComm.Parameters.Add(pParamList[i]);
            }
            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            try
            {
                Str = GSql.GComm.ExecuteScalar().ToString();
                return Str;
            }
            catch (SqlException ex)
            {
                gStrErrMsg = ex.ToString();
                return Str;
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
        }

        public static string ExeScal(EnumServer ServerConn, String pProcedureName)
        {
            String Str = "";
            OpenConnection(ServerConn);

            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            try
            {
                Str = GSql.GComm.ExecuteScalar().ToString();
                return Str;
            }
            catch (SqlException ex)
            {
                gStrErrMsg = ex.ToString();
                return Str;
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
        }

        /// <summary> Execute NonQuery With No of Affected Record With(Store Procedure)
        /// Use To Execute Store Procedure With Parameter List and With General Connetion
        /// </summary>
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>Number of Affected Record Or If error raise then return -1</returns>
        /// 
        public static int ExNonQuery(EnumServer ServerConn, String pProcedureName, SqlParameter[] pParamList)
        {
            OpenConnection(ServerConn);
            Int32 IntParams = pParamList.Length;
            GSql.GComm.Parameters.Clear();

            for (Int32 IntI = 0; IntI < pParamList.Length; IntI++)
            {
                if ((pParamList[IntI] != null)) GSql.GComm.Parameters.Add(pParamList[IntI]);
            }

            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.CommandTimeout = 600;
            try
            {
                return GSql.GComm.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                gStrErrMsg = ("Error :  " + e.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
            return -1;
        }


        public static int ExNonQuery(EnumServer ServerConn, string StrSql, string StrA)
        {
            OpenConnection(ServerConn);

            GSql.GComm.Parameters.Clear();

            GSql.GComm.CommandType = CommandType.Text;
            GSql.GComm.CommandText = StrSql;
            try
            {
                return GSql.GComm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                gStrErrMsg = ("Here Error Is " + e.ToString());
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
            return -1;
        }

 
        public static int ExNonQuery(EnumServer ServerConn, String pProcedureName, SqlParameter[] pParamList, String pStrOutParam)
        {
            OpenConnection(ServerConn);
            Int32 IntParams = pParamList.Length;
            GSql.GComm.Parameters.Clear();

            for (Int32 IntI = 0; IntI < pParamList.Length; IntI++)
            {
                if ((pParamList[IntI] != null)) GSql.GComm.Parameters.Add(pParamList[IntI]);
            }

            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            try
            {
                GSql.GComm.ExecuteNonQuery();
                return Convert.ToInt32("0" + GSql.GComm.Parameters[pStrOutParam].Value);
            }
            catch (SqlException e)
            {
                gStrErrMsg =e.ToString();

            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                CCon(ServerConn);
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServerConn"></param>
        /// <param name="pProcedureName"></param>
        /// <param name="pParamList"></param>
        /// <returns></returns>
        public static int ExNonQuery(EnumServer ServerConn, String pProcedureName, SqlParameter[] pParamList, EnumSqlTran pSqlTransaction)
        {
            Int32 IntParams = pParamList.Length;
            GSql.GComm.Parameters.Clear();

            ///MB
            if (pSqlTransaction == EnumSqlTran.Default)
            {
                OpenConnection(ServerConn);
            }

            if (pSqlTransaction == EnumSqlTran.Start || pSqlTransaction == EnumSqlTran.None)
            {
                OpenConnection(ServerConn);
                GSqlTran = GSql.GComm.Connection.BeginTransaction();
                GSql.GComm.Transaction = GSqlTran;
            }

            for (Int32 IntI = 0; IntI < pParamList.Length; IntI++)
            {
                if ((pParamList[IntI] != null)) GSql.GComm.Parameters.Add(pParamList[IntI]);
            }

            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            try
            {
                return GSql.GComm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                gStrErrMsg= e.ToString();
                GSqlTran.Rollback();
                return -1;
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
                if (pSqlTransaction == EnumSqlTran.Stop)
                {
                    GSqlTran.Commit();
                    CCon(ServerConn);
                }
            }
            return -1;
        }
         

        public static int ExNonQuery(EnumServer ServerConn, String pProcedureName)
        {
            OpenConnection(ServerConn);

            GSql.GComm.CommandType = CommandType.Text;
            GSql.GComm.CommandText = pProcedureName;
            try
            {
                return GSql.GComm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                gStrErrMsg =e.ToString();
            }
            finally
            {
                CCon(ServerConn);
            }
            return -1;
        }


        public static int ExNonQuery(String pProcedureName, SqlParameter[] pParamList)
        {
            Int32 IntParams = pParamList.Length;
            GSql.GComm.Parameters.Clear();

            for (Int32 IntI = 0; IntI < pParamList.Length; IntI++)
            {
                if ((pParamList[IntI] != null)) GSql.GComm.Parameters.Add(pParamList[IntI]);
            }

            GSql.GComm.CommandType = CommandType.StoredProcedure;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.Connection = GSql.GConn;
            try
            {
                return GSql.GComm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                gStrErrMsg =e.ToString();
            }
            finally
            {
                GSql.GComm.Parameters.Clear();
            }
            return -1;
        }
        public static int ExNonQuery(String pProcedureName)
        {
            GSql.GComm.CommandType = CommandType.Text;
            GSql.GComm.CommandText = pProcedureName;
            GSql.GComm.Connection = GSql.GConn;

            try
            {
                return GSql.GComm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                gStrErrMsg = e.ToString();
            }
            finally
            {
            }
            return -1;
        }



        #endregion

        #region Closing And Utility like Reader,RecordSet,Ulility Like HasRow,FindNewID,FindText

        /// <summary>Close An Open Reader(SqlDataReader) 
        /// Closes Open Data Reader
        /// </summary>
        /// <param name="pReader">SqlDataReader</param>
        public static void ClsRed(SqlDataReader pReader)
        {
            if (pReader != null)
            {
                if (pReader.IsClosed == false)
                {
                    pReader.Close();
                }
            }
        }

        /// <summary>Method For Checking Rows In A given Reader 
        /// And Return True If Reader Has Rows Loaded Otherwise Returns False
        /// </summary>
        /// <param name="pReader">SqlDataReader</param>
        /// <returns></returns>
        public static bool HasRows(SqlDataReader pReader)
        {
            if (pReader != null)
            {
                if (pReader.HasRows == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>Finds New Id From A Given Table With Criteria
        /// And Returns New Integer Number
        /// </summary>
        /// <param name="ServerConn">Server Name Enum</param>
        /// <param name="pStrTableName">Table Name</param>
        /// <param name="pStrIdExp">Expression Like Max(ID)</param>
        /// <param name="pStrCriateria">Search Criteria</param>
        /// <returns>Integer</returns>
        public static int FindNewId(EnumServer ServerConn, string pStrTableName, string pStrIdExp, string pStrCriateria)
        {
            OperationSql.Clear();
            Int32 StrRetVal;
            OperationSql.AddParams("Table", pStrTableName);
            OperationSql.AddParams("Field", pStrIdExp);
            if (!string.IsNullOrEmpty(pStrCriateria))
                OperationSql.AddParams("Criteria", " And " + pStrCriateria);
            else
                OperationSql.AddParams("Criteria", " ");
            StrRetVal = Convert.ToInt32(ExeScal(ServerConn, "Sp_FindNewId", OperationSql.GetParams()) + "");
            return StrRetVal + 1;
        }
       
        public static DateTime? DTDBDate(String pStrDateTime)
        {
            CultureInfo CI1 = new CultureInfo("en-GB");
            if (pStrDateTime == null || pStrDateTime == "")
            {
                DateTime? DT = null;
                return DT;
            }
            else
            {
                return DateTime.Parse(pStrDateTime, CI1);
            }
        }
        /// <summary>Finds Text From A Given TableName With Criteria
        /// And Returns Search String
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <param name="pStrTableName">Table Name</param>
        /// <param name="pStrIdExp">Search Expression</param>
        /// <param name="pStrCriateria">Search Criteria</param>
        /// <returns>String</returns>
        public static string FindText(string pStrTableName, string pStrIdExp, string pStrCriateria)
        {
            string StrRetVal = "";
            SqlDataReader mSqlReader;
            AddParams("TableName", pStrTableName);
            AddParams("Fields", pStrIdExp + " Search");
            if (!string.IsNullOrEmpty(pStrCriateria))
                AddParams("Criteria", " And " + pStrCriateria);
            else
                AddParams("Criteria", " ");
            mSqlReader = GOpeSql.ExeRed("SP_SELECT", GetParams());
            if (GOpeSql.HasRows(mSqlReader) == true)
            {
                mSqlReader.Read();
                StrRetVal = mSqlReader["Search"].ToString();
            }
            mSqlReader.Close();
            return StrRetVal;
        }
        #endregion

        #region Utility
        /// <summary>
        /// Method For Display Date In Sql [MM/DD/YYYY] Format
        /// </summary>
        /// <param name="pStrDate">Date String</param>
        /// <returns>String</returns>
        public static string SqlDate(string pStrDate)
        {
            if (pStrDate.Length == 0)
            {
                return "null";
            }
            else
            {
                //return "'" + DateTime.Parse(pStrDate).ToString(new System.Globalization.CultureInfo("en-US", false)).ToString() + "'";
                return "" + DateTime.Parse(pStrDate).ToString("MM/dd/yy") + "";
            }
        }
        /// <summary>
        /// Method For Display Time In Sql [HH:MM AM/PM] Format
        /// </summary>
        /// <param name="pStrTime">Time String</param>
        /// <returns>String</returns>
        private static string SqlTime(string pStrTime)
        {
            if (pStrTime.Length == 0)
            {
                return "null";
            }
            else
            {
                return Convert.ToDateTime(pStrTime).ToString("hh:mm tt");
            }
        }
        /// <summary>
        /// Data Access To Business Layer For Time
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        private static DateTime? DTDBTime(DateTime? pDateTime)
        {
            if (pDateTime == null || pDateTime.ToString() == "")
            {
                DateTime? DT = null; // new DateTime(1, 1, 1); 
                return DT;
            }
            else
            {
                DateTime? DTRet = new DateTime(1, 1, 1, pDateTime.Value.Hour, pDateTime.Value.Minute, pDateTime.Value.Second);
                return DTRet;
                //return DateTime.Parse(pDateTime.ToString("hh:mm tt"));
            }
        }
        /// <summary>
        /// Date Checking
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        private static DateTime? DTDBDate(DateTime? pDateTime)
        {
            if (pDateTime == null || pDateTime.ToString() == "")
            {
                DateTime? DT = null;
                return DT;
            }
            else
            {
                return pDateTime;
            }
        }
        #endregion



        #region Start Transaction, Begin, Commite, Rollback
        public static void ExNonQueryStartTran(EnumServer ServerConn)
        {
            GSql.GComm.Parameters.Clear();
            OpenConnection(ServerConn);
            GSqlTran = GSql.GComm.Connection.BeginTransaction();
            GSql.GComm.Transaction = GSqlTran;
        }

        public static void ExNonQueryCommitTran(EnumServer ServerConn)
        {
            GSql.GComm.Parameters.Clear();
            GSqlTran.Commit();
            CCon(ServerConn);
        }

        public static void ExNonQueryRollbackTran(EnumServer ServerConn)
        {
            GSql.GComm.Parameters.Clear();
            GSqlTran.Rollback();
            CCon(ServerConn);
        }
        #endregion
    }
}