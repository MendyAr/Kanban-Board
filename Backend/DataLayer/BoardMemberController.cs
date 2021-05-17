using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class BoardMemberController 

    {
        private readonly string _connectionString;
        private const string _tableName = "BoardMember";

        public BoardMemberController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            this._connectionString = $"Data Source={path}; Version=3;";            
        }
            
        public HashSet<string> Select(string ID)  // email, set of boards with syntax creatorEmail:boardName
        {
            HashSet<string> results = new HashSet<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} WHERE (ID = @{ID})";
                SQLiteDataReader dataReader = null;

                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    command.Parameters.Add(new SQLiteParameter(ID, ID));

                    while (dataReader.Read())
                    {
                        results.Add(dataReader.GetString(1));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        public void insert(string ID, string userEmail)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{ID}, @{userEmail})"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(ID,ID));
                    command.Parameters.Add(new SQLiteParameter(userEmail, userEmail));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    //log
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }

        public void delete()
        {
        }
    }
}
