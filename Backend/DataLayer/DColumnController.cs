using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataLayer
{
    
    class DColumnController : DalController
    {
        private DTaskController taskController = new DTaskController();
        public DColumnController() : base("Column")
        {

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

        protected override List<DTO> Select()
        {
            List<DColumn> columnsList = base.Select().Cast<DColumn>().ToList();
            List<DTask> tasksList = taskController.Select().Cast<DTask>().ToList();

        }
    }
}
