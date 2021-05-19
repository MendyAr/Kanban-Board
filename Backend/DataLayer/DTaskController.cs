using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DTaskController : DalController <DTask>
    {
        //constructor

        internal DTaskController() : base("Task")
        {}

        // methods

        internal List<DTask> Select(string boardCreator,string boardName,int ordinal)
        {
            List<DTask> results = new List<DTask>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} WHERE (BoardCreator = @{boardCreator} AND BoardName = @{boardName} AND Ordinal = @{ordinal})";

                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(boardCreator,boardCreator));
                    command.Parameters.Add(new SQLiteParameter(boardName, boardName));
                    command.Parameters.Add(new SQLiteParameter(ordinal.ToString(), ordinal));
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch
                {
                    // log
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

        internal void DeleteBoardtask(string creatorEmail, string boardName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName} WHERE (BoardCreator = @{creatorEmail} AND BoardName=@{boardName}";
                command.Parameters.Add(new SQLiteParameter(creatorEmail, creatorEmail));
                command.Parameters.Add(new SQLiteParameter(boardName, boardName));
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

        protected override DTask ConvertReaderToObject(SQLiteDataReader reader)
        {
            int taskId = reader.GetInt32(1);
            DateTime creationTime = DateTime.Parse(reader.GetString(2));
            string title = reader.GetString(3);
            string description = reader.GetString(4);
            DateTime dueDate = DateTime.Parse(reader.GetString(5));
            string assignee = reader.GetString(6);
            int ordinal = reader.GetInt32(7);
            string boardCreator = reader.GetString(8);
            string boardName = reader.GetString(9);
            DTask result = new DTask(taskId, creationTime, title, description, dueDate, assignee, ordinal, boardCreator, boardName);
            return result;
        }
    }
}
