using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoardController : DalController
    {
        DColumnController _columnController;
        BoardMemberController _boardMemberController;

        public DBoardController(DColumnController columnController, BoardMemberController boardMemberController) : base("Board")
        {
            _columnController = columnController;
            _boardMemberController = boardMemberController;
        }

        public override void Insert(DTO dTO)
        {
            DBoard board = (DBoard)dTO;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{board.ID}, @{board.Creator}, @{board.BoardName})"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(board.ID, board.ID));
                    command.Parameters.Add(new SQLiteParameter(board.Creator, board.Creator));
                    command.Parameters.Add(new SQLiteParameter(board.BoardName, board.BoardName));

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

        public List<DTO> select(string boardCreator, string boardName)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} WHERE (Creator = @{boardCreator} AND Name = @{boardName})";

                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.Parameters.Add(new SQLiteParameter(boardCreator, boardCreator));
                    command.Parameters.Add(new SQLiteParameter(boardName, boardName));
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        DBoard board = (DBoard)ConvertReaderToObject(dataReader);
                        IList<DColumn> columns = _columnController.Select(boardCreator, boardName).Cast<DColumn>().ToList();
                        board.Columns= columns;
                        board.Members = _boardMemberController.Select(board.ID);
                        results.Add(board);
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
            


        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            string creator = reader.GetString(1);
            string name = reader.GetString(2);
            return new DBoard(creator, name);
        }  
    }
}
