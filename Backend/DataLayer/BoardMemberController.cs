using System;
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
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
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

        internal void DeleteAll()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName}";

                try
                {
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

        internal void DeleteBoardMembers(string creatorEmail, string boardName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName} WHERE ID = @{creatorEmail + boardName}";
                command.Parameters.Add(new SQLiteParameter(creatorEmail + boardName, creatorEmail + boardName));
                try
                {
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
        public bool Insert(string ID, string userEmail)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{ID}, @{userEmail})"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(ID, ID));
                    command.Parameters.Add(new SQLiteParameter(userEmail, userEmail));
                    connection.Open();
                    result = command.ExecuteNonQuery();
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
            return result > 0;
        }

        public void Delete()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                }
                catch
                {
                    //log
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
        }
    }
}
