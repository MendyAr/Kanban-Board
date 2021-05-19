using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoard : DTO
    {
        private readonly string _creatorEmail;
        private string _boardName;
        private IList<DColumn> columns;
        private HashSet<string> members;
        private BoardMemberController _boardMemberController;

        public string CreatorEmail { get => _creatorEmail;}
 
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

        internal DBoard (string creatorEmail, string boardName) : base(creatorEmail + boardName,"Board")
        {
            this._creatorEmail = creatorEmail;
            this._boardName = boardName;
            columns = new DColumn[3];
            members = new HashSet<string>();
            _boardMemberController = new BoardMemberController();
        }

        internal int numberOfTasks()
        {
            return columns[0].Tasks.Count + columns[1].Tasks.Count + columns[2].Tasks.Count;
        }

        protected override SQLiteCommand InsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO {_tableName}  VALUES (@{ID}, @{CreatorEmail}, @{BoardName})"
            };
            command.Parameters.Add(new SQLiteParameter(ID, ID));
            command.Parameters.Add(new SQLiteParameter(CreatorEmail,CreatorEmail));
            command.Parameters.Add(new SQLiteParameter(BoardName, BoardName));
            return command;
        }

        internal void AddMember (string memberEmail)
        {
            bool addSuccessfully = _boardMemberController.Insert(ID, memberEmail);
            if (addSuccessfully)
                Members.Add(memberEmail);
            else
                throw new System.Exception($"failed in adding {memberEmail} to be member of {BoardName} (of {CreatorEmail})");
        }
    }
}
