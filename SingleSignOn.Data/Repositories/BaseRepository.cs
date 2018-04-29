
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOn.Data.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        private static SqlConnection _connection;

        public BaseRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultDatabase"));
        }

        public virtual T PopulateRecord(SqlDataReader reader)
        {
            return null;
        }
        protected async Task<IEnumerable<T>> GetRecords(SqlCommand command)
        {
            var list = new List<T>();
            command.Connection = _connection;
            await _connection.OpenAsync();

            try
            {
                var reader = await command.ExecuteReaderAsync();
                try
                {
                    while (reader.Read())
                        list.Add(PopulateRecord(reader));
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return list;
        }
        protected async Task<T> GetRecord(SqlCommand command)
        {
            T record = null;
            command.Connection = _connection;
            await _connection.OpenAsync();
            try
            {
                var reader = await command.ExecuteReaderAsync();
                try
                {
                    while (reader.Read())
                    {
                        record = PopulateRecord(reader);
                        break;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return record;
        }

        protected async Task<bool> SaveRecord(SqlCommand command)
        {
            command.Connection = _connection;
            await _connection.OpenAsync();
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch
            {
                return false;
            }
            finally
            {
                _connection.Close();
            }

            return true;
        }

    }
}
