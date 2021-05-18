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
    }
}
