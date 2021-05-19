using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DColumnController : DalController <DColumn>
    {
        // field
        private DTaskController _taskController;

        // controller
        internal DColumnController() : base("Column")
        {
            _taskController = new DTaskController();
        }

        // methods

        internal List<DColumn> Select(string boardCreator, string boardName)
        {
            List<DColumn> results = new List<DColumn>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} WHERE (BoardCreator = @{boardCreator} AND BoardName = @{boardName})";

                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(boardCreator, boardCreator));
                    command.Parameters.Add(new SQLiteParameter(boardName, boardName));
                    dataReader = command.ExecuteReader();
                    
                    while (dataReader.Read())
                    {
                        DColumn column = ConvertReaderToObject(dataReader);
                        List<DTask> tasks = _taskController.Select(boardCreator, boardName,column.Ordinal).ToList();
                        column.Tasks = tasks;
                        results.Add(column);
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

        internal override void DeleteAll()
        {
            base.DeleteAll();
            _taskController.DeleteAll();
        }

        internal void DeleteBoardColumns(string creatorEmail, string boardName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName} WHERE (Creator = @{creatorEmail} AND Board=@{boardName}";
                command.Parameters.Add(new SQLiteParameter(creatorEmail, creatorEmail));
                command.Parameters.Add(new SQLiteParameter(boardName,boardName));
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
            _taskController.DeleteBoardtask(creatorEmail, boardName);
        }

        protected override DColumn ConvertReaderToObject(SQLiteDataReader reader)
        {
            string creator = reader.GetString(1);
            string boardName = reader.GetString(2);
            int ordinal = reader.GetInt32(3);
            int limit = reader.GetInt32(4);

            DColumn result = new DColumn(creator, boardName, ordinal, limit);
            return result;
        }
    }
}
