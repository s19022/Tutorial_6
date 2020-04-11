using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial_6.Services
{
    public class MsSqlStudentDbService : IStudentDbService
    {
        private readonly string _connectionString = "Data Source=db-mssql;Initial Catalog=s19022;Integrated Security=True";
        public bool Contains(string index)
        {
            bool result;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "Select 1 from student where index = @index";
                command.Parameters.AddWithValue("index", index);
                connection.Open();
                var dr = command.ExecuteReader();

                result =  dr.Read();
            }
            return result;
        }
    }
}
