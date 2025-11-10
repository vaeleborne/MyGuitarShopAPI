using Microsoft.Data.SqlClient;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{

    /// <summary>
    /// Data type to be used to represent a sql parameter which will be used
    /// in the more generic helper functions in RepoHelpers
    /// </summary>
    /// <param name="Name">Name of Parameter. i.e. "@ID"</param>
    /// <param name="Type">The sql type to be verified. i.e. SqlDbType.Int</param>
    /// <param name="Value">The value for that parameter</param>
    public record SqlParameterModel(
        string Name, 
        SqlDbType Type,
        object Value);

    /// <summary>
    /// Static Class containing helper functions for the repos. 
    /// </summary>
    internal static class RepoHelpers
    {
        /// <summary>
        /// Creates a reader from a connection factory and sql command
        /// </summary>
        /// <param name="connectionFactory">The connection factory to be used.</param>
        /// <param name="cmd">The sql command string to be used.</param>
        /// <param name="parameters">Enumarable of any parameters needed. Null if not given.</param>
        /// <returns>
        /// Reader object
        /// </returns>
        /// <exception cref="Exception">Throws what the exception message was</exception>
        public static async Task<SqlDataReader> ConnectAndGetReader(
            SqlConnectionFactory connectionFactory,
            string cmd,
           IEnumerable<SqlParameterModel>? parameters = null)
        {
            try
            {
                var conn =  await connectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(cmd, conn);

                if(parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        var param = command.Parameters.Add(p.Name, p.Type);
                        param.Value = p.Value ?? DBNull.Value;
                    }
                }
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }


        public static async Task<int> ConnectAndExecuteNonQuery(
            SqlConnectionFactory connectionFactory,
            string cmd,
            IEnumerable<SqlParameterModel>? parameters = null
            )
        {
            try
            {
                var conn = await connectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(cmd, conn);

                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        var param = command.Parameters.Add(p.Name, p.Type);
                        param.Value = p.Value ?? DBNull.Value;
                    }
                }
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
