using Microsoft.Data.SqlClient;
namespace MyGuitarShop.Data.Ado.Factories
{
    public class SqlConnectionFactory(string connection_string)
    {
        public SqlConnection OpenSqlConnection()
        {
            var connection = new SqlConnection(connection_string);
            connection.Open();
            return connection; 
        }

    }
}
