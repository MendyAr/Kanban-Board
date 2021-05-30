using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DColumn : DTO
    {
        // properties

        private string _name;
        private string _creatorEmail;
        private string _boardName;
        private int _ordinal;
        private int _limit;
        private List<DTask> _tasks;

        private const string COL_NAME = "Creator";
        private const string COL_CREATOR_EMAIL = "Creator";
        private const string COL_BOARD_NAME = "Board";
        private const string COL_LIMIT = "Limit";
        private const string COL_ORDINAL = "Ordinal";

        internal string Name { get => _name; 
            set {
                if (Persist)
                {
                    Update(COL_NAME, value);
                }
                _name = value;
            } }

        internal string CreatorEmail
        {
            get => _creatorEmail; set
            {
                if (Persist)
                {
                    Update(COL_CREATOR_EMAIL, value);
                }
                _creatorEmail = value;
            }
        }

        internal string BoardName
        {
            get => _boardName; set
            {
                if (Persist)
                {
                    Update(COL_BOARD_NAME, value);
                }
                _boardName = value;
            }
        }

        internal int Limit
        {
            get => _limit; set
            {
                if (Persist)
                {
                    Update(COL_LIMIT, value);
                }
                _limit = value;
            }
        }
        internal int Ordinal { get => _ordinal; 
            set {
                if (Persist)
                {
                    
                    Update(COL_ORDINAL, value);
                }
                foreach (DTask task in _tasks)
                {
                    task.Ordinal = value;
                }
                _ordinal = value;
                }
        } 

        internal List<DTask> Tasks
        {
            get => _tasks; set
            {
                _tasks = value;
            }
        }


        // constructor

        internal DColumn(string name,string creatorEmail, string boardName, int ordinal, int limit) : base(creatorEmail + boardName + ordinal, "Column")
        {
            _creatorEmail = creatorEmail;
            _boardName = boardName;
            _ordinal = ordinal;
            _limit = limit;
            _name = name;
        }

        protected override SQLiteCommand InsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO {_tableName}  VALUES (@{COL_ID},@{COL_NAME}, @{COL_CREATOR_EMAIL}, @{COL_BOARD_NAME}, @{COL_ORDINAL}, @{COL_LIMIT})"
            };
            command.Parameters.Add(new SQLiteParameter(COL_ID, Id));
            command.Parameters.Add(new SQLiteParameter(COL_NAME, _name));
            command.Parameters.Add(new SQLiteParameter(COL_CREATOR_EMAIL, CreatorEmail));
            command.Parameters.Add(new SQLiteParameter(COL_BOARD_NAME, BoardName));
            command.Parameters.Add(new SQLiteParameter(COL_ORDINAL, Ordinal));
            command.Parameters.Add(new SQLiteParameter(COL_LIMIT, Limit));
            return command;
        }
    }
}
