using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryADO.NET
{
    public class Repository : IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly string _tableName = "Records";

        public Repository(string conStr) : this(new SqlConnection(conStr ?? throw new ArgumentNullException(nameof(conStr))))
        { }

        public Repository(SqlConnection connection) =>
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public void Dispose() => _connection.Dispose();


        public async Task<List<Record>> GetRecords()
        {
            var commandString = $"SELECT Id, Text, Author, RecordDate FROM {_tableName}";

            var command = new SqlCommand(commandString, _connection);
            try
            {
                await _connection.OpenAsync();

                var result = new List<Record>();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Record
                        (
                            id: (int) reader["Id"],
                            text: (string) reader["Text"],
                            author: (string) reader["Author"],
                            recoredDate: (DateTime)reader["RecordDate"]
                        ));
                    }
                }

                return result;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return new List<Record>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Record>();
            }
            finally
            {
                _connection.Close();
            }

        }

        public async Task CreateRecord(Record record)
        {
            if (record is null) throw new ArgumentNullException(nameof(record));

            var query = $@"INSERT INTO {_tableName} (Text, Author, RecordDate)
                           VALUES (@Text, @Author, @RecordDate)";

            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Text", record.Text);
            command.Parameters.AddWithValue("@Author", record.Author);
            command.Parameters.AddWithValue("@RecordDate", record.RecordDate);
            try
            {
                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task DeleteRecord(Record record)
        {
            if (record is null) throw new ArgumentNullException(nameof(record));

            var query = $"DELETE FROM {_tableName} WHERE Id = @DeletedId";
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@DeletedId", record.Id);
            try
            {
                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task UpdateRecord(Record record)
        {
            if (record is null) throw new ArgumentNullException(nameof(record));

            var query = $@"UPDATE {_tableName} 
                        SET Text = @Text, Author = @Author, RecordDate = @RecordDate 
                        WHERE Id = @UpdatedId";
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Text", record.Text);
            command.Parameters.AddWithValue("@Author", record.Author);
            command.Parameters.AddWithValue("@RecordDate", record.RecordDate);
            command.Parameters.AddWithValue("@UpdatedId", record.Id);
            try
            {
                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
