using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTaskController : DalController
    {
        public DTaskController() : base("Task")
        {}
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            int taskId = reader.GetInt32(0);
            DateTime creationTime = DateTime.Parse(reader.GetString(1));
            string title = reader.GetString(2);
            string description = reader.GetString(3);
            DateTime dueDate = DateTime.Parse(reader.GetString(4));
            string assignee = reader.GetString(5);

            DTask result = new DTask(taskId,creationTime,title,description,dueDate,assignee);
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
    }
    
}
