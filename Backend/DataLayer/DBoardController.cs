using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

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
                    CommandText = $"INSERT INTO {_tableName}  VALUES (@{board.ID}, @{board.CreatorEmail}, @{board.BoardName})"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(board.ID, board.ID));
                    command.Parameters.Add(new SQLiteParameter(board.CreatorEmail, board.CreatorEmail));
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

        public override List<DTO> Select()
        {
            List<DTO> results = base.Select(); 

            foreach(DTO dto in results)
            {
                DBoard board = (DBoard) dto;
                IList<DColumn> columns = _columnController.Select(board.CreatorEmail, board.BoardName).Cast<DColumn>().ToList();
                board.Columns= columns;
                board.Members = _boardMemberController.Select(board.ID);
                results.Add(board); 
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
