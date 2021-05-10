using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTaskController : DalController
    {
        public DTaskController() : base("Task")
        {}
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
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

            DTask result = new DTask(taskId,creationTime,title,description,dueDate,assignee,ordinal,boardCreator,boardName);
            return result;
        }

        public override void Insert(DTO dTO)
        {
            DTask tesk = (DTask)dTO;             
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{tesk.ID},@{tesk.TaskId}, @{tesk.CreationTime.ToString()}, @{tesk.Title}, @{tesk.Description}, @{tesk.DueDate.ToString()}, @{tesk.Assignee})" 
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(tesk.ID, tesk.ID));
                    command.Parameters.Add(new SQLiteParameter(tesk.TaskId.ToString(), tesk.TaskId));
                    command.Parameters.Add(new SQLiteParameter(tesk.CreationTime.ToString(), tesk.CreationTime));
                    command.Parameters.Add(new SQLiteParameter(tesk.Title, tesk.Title));
                    command.Parameters.Add(new SQLiteParameter(tesk.Description, tesk.Description));
                    command.Parameters.Add(new SQLiteParameter(tesk.DueDate.ToString(), tesk.DueDate));
                    command.Parameters.Add(new SQLiteParameter(tesk.Assignee, tesk.Assignee));
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

        public List<DTO> Select(string boardCreator,string boardName)

        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} WHERE (BoardCreator = @{boardCreator} AND BoardName = @{boardName})";

                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(boardCreator,boardCreator));
                    command.Parameters.Add(new SQLiteParameter(boardName, boardName));
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

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
    }
    
}
