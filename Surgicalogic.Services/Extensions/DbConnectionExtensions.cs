using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Data.SqlClient;

namespace Surgicalogic.Services.Extensions
{
    public static class DbConnectionExtensions
    {
        public static async Task<int> ExecuteNonQueryAndGetInsertedIdAsync(this IDbConnection connection, string query, string returnValue = "id")
        {
            if (connection is SqlConnection dbConnection)
            {
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = query;

                    var outputParameter = new SqlParameter(returnValue, SqlDbType.Decimal);

                    command.Parameters.Add(outputParameter);

                    await command.ExecuteNonQueryAsync();

                    return int.Parse(outputParameter.Value.ToString());
                }
            }

            return 0;
        }
    }
}
