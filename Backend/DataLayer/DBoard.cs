using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DBoard : DTO
    {

        //properties

        private readonly string _creatorEmail;
        private string _boardName;
        private IList<DColumn> columns;
        private HashSet<string> members;
        private BoardMemberController _boardMemberController;

        private const string COL_BOARD_NAME = "Name";
        private const string COL_CREATOR_EMAIL = "Creator";


        internal string CreatorEmail { get => _creatorEmail;}

        internal string BoardName { get => _boardName; set 
            {

                if (Persist)
                {
                    Update(COL_BOARD_NAME, value);
                }
                _boardName = value;
            } }

        internal IList<DColumn> Columns { get => columns; set => columns = value; }

        internal HashSet<string> Members { get => members; set => members = value; }

        //constructor
        internal DBoard (string creatorEmail, string boardName) : base(creatorEmail + boardName,"Board")
        {
            this._creatorEmail = creatorEmail;
            this._boardName = boardName;
            columns = new DColumn[3];
            members = new HashSet<string>();
            _boardMemberController = new BoardMemberController();
        }

        //methods
        internal int numberOfTasks()
        {
            return columns[0].Tasks.Count + columns[1].Tasks.Count + columns[2].Tasks.Count;
        }

        internal void AddMember (string memberEmail)
        {
            bool addSuccessfully = _boardMemberController.Insert(Id, memberEmail);
            if (addSuccessfully)
                Members.Add(memberEmail);
            else
                throw new System.Exception($"failed in adding {memberEmail} to be member of {BoardName} (of {CreatorEmail})");
        }

        protected override SQLiteCommand InsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO {_tableName}  VALUES (@{COL_ID}, @{COL_CREATOR_EMAIL}, @{COL_BOARD_NAME})"
            };
            command.Parameters.Add(new SQLiteParameter(COL_ID, Id));
            command.Parameters.Add(new SQLiteParameter(COL_CREATOR_EMAIL, CreatorEmail));
            command.Parameters.Add(new SQLiteParameter(COL_BOARD_NAME, BoardName));
            return command;
        }
    }

}
