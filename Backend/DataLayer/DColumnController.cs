using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    
    class DColumnController : DalController
    {
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
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{column.ID}, @{column.Creator}, @{column.Board}, @{column.Ordinal})"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(column.ID, column.ID));
                    command.Parameters.Add(new SQLiteParameter(column.Creator, column.Creator));
                    command.Parameters.Add(new SQLiteParameter(column.Board, column.Board));
                    command.Parameters.Add(new SQLiteParameter(column.Ordinal.ToString(), column.Ordinal));
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
            throw new NotImplementedException();
        }
    }
}
