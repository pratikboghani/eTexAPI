using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace eTexAPI.Data.Common
{
    public interface IDapperHelper : IDisposable
    {
        DbConnection GetConnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
       
        Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> GetMultiple<T1, T2>(string sql, DynamicParameters parameters,
                                        Func<GridReader, IEnumerable<T1>> func1,
                                        Func<GridReader, IEnumerable<T2>> func2);

        Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> GetMultiple<T1, T2, T3>(string sql, DynamicParameters parameters,
                                       Func<GridReader, IEnumerable<T1>> func1,
                                       Func<GridReader, IEnumerable<T2>> func2,
                                       Func<GridReader, IEnumerable<T3>> func3);
        List<object> getMultiple(string sql, DynamicParameters parameters, params Func<GridReader, object>[] readerFuncs);
        DataTable GetDataSet(string sp, SqlParameter[] parms, CommandType command = CommandType.StoredProcedure);
      
    }
}
