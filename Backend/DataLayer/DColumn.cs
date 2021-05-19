using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DColumn : DTO
    {
        // properties

        private string _creatorEmail;
        private string _boardName;
        private readonly int _ordinal;
        private int _limit;
        private List<DTask> _tasks;

        private const string CREATOR = "Creator";
        private const string BOARD = "Board";
        private const string LIMIT = "Limit";


        internal string CreatorEmail
        {
            get => _creatorEmail; set
            {
                if (Persist)
                {
                    Update(CREATOR, value);
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
                    Update(BOARD, value);
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
                    Update(LIMIT, value);
                }
                _limit = value;
            }
        }
        internal int Ordinal { get => _ordinal; }

        internal List<DTask> Tasks
        {
            get => _tasks; set
            {
                _tasks = value;
            }
        }


        // constructor

        internal DColumn(string creatorEmail, string boardName, int ordinal, int limit) : base(creatorEmail + boardName + ordinal, "Column")
        {
            _creatorEmail = creatorEmail;
            _boardName = boardName;
            _ordinal = ordinal;
            _limit = limit;
        }


        // method

        protected override SQLiteCommand InsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO {_tableName}  VALUES (@{ID}, @{CreatorEmail}, @{BoardName}, @{Ordinal}, @{Limit})"
            };
            command.Parameters.Add(new SQLiteParameter(ID, ID));
            command.Parameters.Add(new SQLiteParameter(CreatorEmail, CreatorEmail));
            command.Parameters.Add(new SQLiteParameter(BoardName, BoardName));
            command.Parameters.Add(new SQLiteParameter(Ordinal.ToString(), Ordinal));
            command.Parameters.Add(new SQLiteParameter(Limit.ToString(),Limit));
            return command;
        }
    }
}
