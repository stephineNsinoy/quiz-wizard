﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace QuizApi.Context
{
    public class DapperContext
    {
        private string _connectionString;

        /// <summary>
        /// Create new instance of DapperContext.
        /// </summary>
        /// <param name="configuration">Connection string to the target database</param>
        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        /// <summary>
        /// Creates a new connection to the database.
        /// </summary>
        /// <returns>The db connection</returns>
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
