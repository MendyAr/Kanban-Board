using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoardController : DalController
    {
        DColumnController _columnController;
        BoardMemberController _boardMemberController;

        public DBoardController() : base("Board")
        {
            _columnController = new DColumnController();
            _boardMemberController = new BoardMemberController();
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

        public override void DeleteAll()
        {
            base.DeleteAll();
            _columnController.DeleteAll();
            _boardMemberController.DeleteAll();
        }

        public void DeleteBoard(string creatorEmail, string boardName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName} WHERE ID = @{creatorEmail + boardName}";
                command.Parameters.Add(new SQLiteParameter(creatorEmail +boardName, creatorEmail + boardName));
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
            _boardMemberController.DeleteBoardMembers(creatorEmail, boardName);
            _columnController.DeleteBoardColumns(creatorEmail, boardName);
        }
    }
}
