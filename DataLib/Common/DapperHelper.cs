using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;
using System.Configuration;
using EncD = DhKaalPs.EncDec.EncDecAlg;
using Ope = eTexAPI.Data.OperationSql;

namespace eTexAPI.Data.Common
{
    public class DapperHelper : OperationSql, IDapperHelper
    {
        private readonly string _ConnectionString;

        public DapperHelper()
        {
            //string LicServer =  ConfigurationSettings.AppSettings.Get("LicServer").ToString();
            //string _user = ConfigurationSettings.AppSettings.Get("Server").ToString();
            //string _pass = ConfigurationSettings.AppSettings.Get("Server").ToString();
            //string _database = ConfigurationSettings.AppSettings.Get("Server").ToString();            
            //_ConnectionString = $"Data Source={_server};Initial Catalog={_database};Persist Security Info=True;User ID={_user};Password={_pass}";

            _ConnectionString = ConfigurationSettings.AppSettings.Get("Server").ToString();
            _ConnectionString += ";Password=" + EncD.DecriptedString(ConfigurationSettings.AppSettings.Get("nCorePS").ToString());
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_ConnectionString);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                return db.Execute(sp, parms, commandType: commandType);
            }
        }

        public async Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                int output = await db.ExecuteAsync(sp, parms, commandType: commandType);
                return output;
            }
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                return db.Query<T>(sp, parms, commandType: commandType).ToList();
            }
        }

        public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                IEnumerable<T> output = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                return output.ToList();
            }
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                IEnumerable<T> output = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                return output.FirstOrDefault();
            }
        }

        //public DataTable GetDataSet(string sp, SqlParameter[] parms, CommandType command = CommandType.StoredProcedure)
        //{ 
        //    using (DataTable dt = new DataTable())
        //    {
        //        using (SqlConnection connection = new SqlConnection(_ConnectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand(sp, connection)
        //            {
        //                CommandType = command
        //            };
        //            cmd.Parameters.AddRange(parms);
        //            try
        //            {
        //                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //                {
        //                    da.Fill(dt);
        //                    return dt;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //            finally
        //            {
        //                cmd.Dispose();
        //                cmd = null;
        //            }
        //        }
        //    }
        //}
        public DataTable GetDataSet(string sp, SqlParameter[] pParamList, CommandType command = CommandType.StoredProcedure)
        {

            using (DataTable dt = new DataTable())
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sp, connection)
                    {
                        CommandType = command
                    };
                    cmd.Parameters.AddRange(pParamList);
                    try
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                }
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> GetMultiple<T1, T2>(string sql, DynamicParameters parameters, Func<SqlMapper.GridReader, IEnumerable<T1>> func1, Func<SqlMapper.GridReader, IEnumerable<T2>> func2)
        {
            List<object> objs = getMultiple(sql, parameters, func1, func2);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> GetMultiple<T1, T2, T3>(string sql, DynamicParameters parameters, Func<SqlMapper.GridReader, IEnumerable<T1>> func1, Func<SqlMapper.GridReader, IEnumerable<T2>> func2, Func<SqlMapper.GridReader, IEnumerable<T3>> func3)
        {
            List<object> objs = getMultiple(sql, parameters, func1, func2, func3);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>);
        }

        public List<object> getMultiple(string sql, DynamicParameters parameters, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            List<object> returnResults = new List<object>();
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                GridReader gridReader = db.QueryMultiple(sql, parameters, commandType: CommandType.StoredProcedure);

                foreach (Func<GridReader, object> readerFunc in readerFuncs)
                {
                    object obj = readerFunc(gridReader);
                    returnResults.Add(obj);
                }
            }
            return returnResults;
        }
         

        public string SearchText(string pStrSQL)
        {
            using (IDbConnection db = new SqlConnection(_ConnectionString))
            {
                var RetFld = db.ExecuteScalar<string>(pStrSQL);
                return RetFld;
            }
        }
         
    }
}
