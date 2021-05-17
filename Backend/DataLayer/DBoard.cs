

using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoard : DTO
    {
        
        private readonly string _creator;
        private string _boardName;
        private IList<DColumn> columns;
        private HashSet<string> members;
        public string Creator { get => _creator;}
 
        public string BoardName { get => _boardName; set 
            {

                if (Persist)
                {
                    Update("Name", value);
                }
                _boardName = value;
            } }

        public IList<DColumn> Columns { get => columns; set => columns = value; }

        public HashSet<string> Members { get => members; set => members = value; }

        public DBoard (string creator, string boardName) : base(new DBoardController(new DColumnController(),new BoardMemberController()),creator + boardName)
        {
            this._creator = creator;
            this._boardName = boardName;
            columns = new DColumn[3];
        }

        public int numberOfTasks()
        {
            return columns[0].Tasks.Count + columns[1].Tasks.Count + columns[2].Tasks.Count;
        }
    }
}
