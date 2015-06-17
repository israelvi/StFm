using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace InfrastructureLayer.Report
{
    public static class ReadDataTable
    {
        public static DataTable GetDataTable(string sqlQuery, string connString = "EntityConnection")
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connString].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }
    }
}
