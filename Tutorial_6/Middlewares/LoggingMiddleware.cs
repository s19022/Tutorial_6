using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tutorial_6.Services;

namespace Tutorial_6.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Index"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing index key");
                return;
            }

            string index = context.Request.Headers["Index"].ToString();

            if (!Contains(index))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Wrong index number");
                return;
            }
            await _next(context);
        }

        private bool Contains(string index)
        {
            bool result;
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19022;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "Select 1 from student where indexNumber = @index";
                command.Parameters.AddWithValue("index", index);
                connection.Open();
                var dr = command.ExecuteReader();

                result = dr.Read();
            }
            return result;
        }
    }


}

