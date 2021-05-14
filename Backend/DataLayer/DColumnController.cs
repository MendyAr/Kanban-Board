using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataLayer
{
    
    class DColumnController : DalController
    {
        private DTaskController taskController;
        public DColumnController() : base("Column")
        {
            taskController = new DTaskController();
        }
        public override void Insert(DTO dTO)
        {
            DColumn column = (DColumn)dTO;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{column.ID}, @{column.Creator}, @{column.Board}, @{column.Ordinal}, @{column.Limit})"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(column.ID, column.ID));
                    command.Parameters.Add(new SQLiteParameter(column.Creator, column.Creator));
                    command.Parameters.Add(new SQLiteParameter(column.Board, column.Board));
                    command.Parameters.Add(new SQLiteParameter(column.Ordinal.ToString(), column.Ordinal));
                    command.Parameters.Add(new SQLiteParameter(column.Limit.ToString(), column.Limit));
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

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            string creator = reader.GetString(1);
            string boardName = reader.GetString(2);
            int ordinal = reader.GetInt32(3);
            int limit = reader.GetInt32(4);

            DColumn result = new DColumn(creator,boardName,ordinal,limit);
            return result;
        }

        public List<DTO> Select(string boardCreator, string boardName)
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
                    command.Parameters.Add(new SQLiteParameter(boardCreator, boardCreator));
                    command.Parameters.Add(new SQLiteParameter(boardName, boardName));
                    dataReader = command.ExecuteReader();
                    
                    while (dataReader.Read())
                    {
                        DColumn column = (DColumn)ConvertReaderToObject(dataReader);
                        List<DTask> tasks = taskController.Select(boardCreator, boardName,column.Ordinal).Cast<DTask>().ToList();
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
    }
}
