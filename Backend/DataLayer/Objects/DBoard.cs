using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DBoard : DTO
    {

        //properties

        private readonly string _creatorEmail;
        private string _boardName;
        private IList<DColumn> _columns;
        private HashSet<string> _members;
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

        internal IList<DColumn> Columns { get => _columns; set => _columns = value; }

        internal HashSet<string> Members { get => _members; set => _members = value; }

        //constructor
        internal DBoard (string creatorEmail, string boardName) : base(creatorEmail + boardName,"Board")
        {
            this._creatorEmail = creatorEmail;
            this._boardName = boardName;
            _columns = new DColumn[3];
            _members = new HashSet<string>();
            _boardMemberController = new BoardMemberController();
        }

        //methods
        internal int numberOfTasks()
        {
            return _columns[0].Tasks.Count + _columns[1].Tasks.Count + _columns[2].Tasks.Count;
        }

        internal void AddMember (string memberEmail)
        {
             _boardMemberController.Insert(Id, memberEmail);
             Members.Add(memberEmail);
        }

        internal void RemoveColumn(int columnOrdinal)
        {
            foreach (DColumn column in _columns)
            {
                if(column.Ordinal == columnOrdinal)
                {
                    column.Remove();
                    break;
                }
            }
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
