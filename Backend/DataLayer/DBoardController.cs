using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DBoardController : DalController <DBoard>
    {
        DColumnController _columnController;
        BoardMemberController _boardMemberController;

        internal DBoardController() : base("Board")
        {
            _columnController = new DColumnController();
            _boardMemberController = new BoardMemberController();
        }

        
        internal override List<DBoard> Select()
        {
            List<DBoard> results = base.Select(); 

            foreach(DBoard dBoard in results)
            {
                IList<DColumn> columns = _columnController.Select(dBoard.CreatorEmail, dBoard.BoardName).Cast<DColumn>().ToList();
                dBoard.Columns= columns;
                dBoard.Members = _boardMemberController.Select(dBoard.ID);
                results.Add(dBoard); 
            }

            return results;
        }

        internal override void DeleteAll()
        {
            base.DeleteAll();
            _columnController.DeleteAll();
            _boardMemberController.DeleteAll();
        }

        internal void DeleteBoard(string creatorEmail, string boardName)
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

        protected override DBoard ConvertReaderToObject(SQLiteDataReader reader)
        {
            string creator = reader.GetString(1);
            string name = reader.GetString(2);
            return new DBoard(creator, name);
        }
    }
}
