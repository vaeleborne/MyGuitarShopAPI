/**
 * @file    SqlConnectionFactory.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a SqlConnection Factory for connecting to SQL DBs.
 * @date    2025-11-11
 * @version 1.0
 *
 * This file contains a factory class for connecting to a Sql database
 */
using Microsoft.Data.SqlClient;
namespace MyGuitarShop.Data.Ado.Factories
{
    /// <summary>
    /// This class acts as a factory to open a sql connection
    /// to a given database, given a connection string
    /// </summary>
    /// <param name="connection_string">The string used to connect to a database</param>
    public class SqlConnectionFactory(string connection_string)
    {
        /// <summary>
        /// Opens a sql connection synchronously
        /// </summary>
        /// <returns>A SqlConnection</returns>
        public SqlConnection OpenSqlConnection()
        {
            var connection = new SqlConnection(connection_string);
            connection.Open();
            return connection; 
        }

        /// <summary>
        /// Opens a sql connection asynchronously
        /// </summary>
        /// <returns>A SqlConnection</returns>
        public async Task<SqlConnection> OpenSqlConnectionAsync()
        {
            var connection =  new SqlConnection(connection_string);
            await connection.OpenAsync();
            return connection;
        }

    }
}
