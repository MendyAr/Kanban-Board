using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class BoardMemberController 
    {

        // properties

        private readonly string _connectionString;
        private const string _tableName = "BoardMember";
        private const string CAL_ID = "ID";
        private const string CAL_USER = "User";

        // constructor

        internal BoardMemberController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";            
        }


        // methods

        internal HashSet<string> Select(string ID)  // email, set of boards with syntax creatorEmail:boardName
        {
            HashSet<string> results = new HashSet<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} WHERE ({CAL_ID} = @{CAL_ID})";
                SQLiteDataReader dataReader = null;

                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    command.Parameters.Add(new SQLiteParameter(CAL_ID, ID));

                    while (dataReader.Read())
                    {
                        results.Add(dataReader.GetString(1));
                    }
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
                command.CommandText = $"DELETE FROM {_tableName} WHERE {CAL_ID} = @{CAL_ID}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(CAL_ID, creatorEmail + boardName));
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
        internal bool Insert(string ID, string userEmail)
        {
            bool failed = false;
            int result = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{CAL_ID}, @{CAL_USER})"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(CAL_ID, ID));
                    command.Parameters.Add(new SQLiteParameter(CAL_USER, userEmail));
                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
                catch
                {
                    //log
                    failed = true;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    if (failed)
                        throw new InvalidOperationException();
                }
                
            }
            return result > 0;
        }

        internal void Delete()
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
