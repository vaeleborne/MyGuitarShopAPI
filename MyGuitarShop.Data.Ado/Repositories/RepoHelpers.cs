/**
 * @file    RepoHelpers.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines some helper functions that get used accross multiple repos.
 * @date    2025-11-11
 * @version 1.0
 */
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
            //Attempt to connect to the db and retrieve a Reader
            try
            {
                //Setup connection with query command
                var conn =  await connectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(cmd, conn);

                //Add parameters to the command if any exist
                if(parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        var param = command.Parameters.Add(p.Name, p.Type);
                        param.Value = p.Value ?? DBNull.Value;
                    }
                }

                //Execute the query, returning the reader and closing the db connection
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex) 
            {
                //On error, throw the error to the caller for it to handle
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Opens connection to DB to execute a non query type of command
        /// </summary>
        /// <param name="connectionFactory">Factory to use for the SQL connection</param>
        /// <param name="cmd">SQL text to execute.</param>
        /// <param name="parameters">Enumarable of SQL parameters for validation</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<int> ConnectAndExecuteNonQuery(
            SqlConnectionFactory connectionFactory,
            string cmd,
            IEnumerable<SqlParameterModel>? parameters = null
            )
        {
            //Try to connect and execute the command
            try
            {
                //Setup Connection and SQL command.
                var conn = await connectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(cmd, conn);

                //Add parameters to the SqlCommand, if any exist
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        var param = command.Parameters.Add(p.Name, p.Type);
                        param.Value = p.Value ?? DBNull.Value;
                    }
                }

                //Run the command.
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                //On error, throw the error to the caller for it to handle
                throw new Exception(ex.Message, ex);
            }
        }


    }
}
