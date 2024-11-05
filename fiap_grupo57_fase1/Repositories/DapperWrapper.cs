using Dapper;
using fiap_grupo57_fase1.Interfaces.Dapper;
using System.Data;

namespace fiap_grupo57_fase1.Repositories
{
    public class DapperWrapper : IDapperWrapper
    {
        public async Task<int> QuerySingleAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.QuerySingleAsync<int>(sql, param);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public IEnumerable<T> Query<T>(IDbConnection connection, string sql, object param = null)
        {
            return connection.Query<T>(sql, param);
        }

        public async Task ExecuteAsync(IDbConnection connection, string sql, object param = null)
        {
            await connection.ExecuteAsync(sql, param);
        }
    }
}
