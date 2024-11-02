using System.Data;

namespace fiap_grupo58_fase1.Interfaces.Dapper
{
    public interface IDapperWrapper
    {
        Task<int> QuerySingleAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
        IEnumerable<T> Query<T>(IDbConnection connection, string sql, object param = null);
        Task ExecuteAsync(IDbConnection connection, string sql, object param = null);
    }
}
