using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTaskController : DalController
    {
        private static string _taskIdColumnName = "TaskId";
        private static string _creationTimeColumnName = "CreationTime";
        private static string _titleColumnName = "Title";
        private static string _descriptionColumnName = "Description";
        private static string _dueDateColumnName = "DueDate";
        private static string _assigneeColumnName = "Assignee";

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

        public bool Insert(DTask tesk)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName} ({_taskIdColumnName} ,{_creationTimeColumnName}, {_titleColumnName}, {_descriptionColumnName}, {_dueDateColumnName}, {_assigneeColumnName}) VALUES ({tesk.TaskId}, {tesk.CreationTime.ToString()}, {tesk.Title}, {tesk.Description}, {tesk.DueDate.ToString()}, {tesk.Assignee})" 
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
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
            return res > 0;
        }
    }
    
}
