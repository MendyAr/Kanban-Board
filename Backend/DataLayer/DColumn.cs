using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DColumn : DTO
    {
        private string _creatorEmail;
        private string _boardName;
        private readonly int _ordinal;
        private int _limit;
        private List<DTask> _tasks;
        private const string _tableName = "Column";

        public string CreatorEmail
        {
            get => _creatorEmail; set
            {
                if (Persist)
                {
                    Update("Creator", value);
                }
                _creatorEmail = value;
            }
        }

        public string BoardName
        {
            get => _boardName; set
            {
                if (Persist)
                {
                    Update("Board", value);
                }
                _boardName = value;
            }
        }

        public int Limit
        {
            get => _limit; set
            {
                if (Persist)
                {
                    Update("Limit", value);
                }
                _limit = value;
            }
        }
        public int Ordinal { get => _ordinal; }

        public List<DTask> Tasks
        {
            get => _tasks; set
            {
                _tasks = value;
            }
        }

        internal DColumn(string creatorEmail, string boardName, int ordinal, int limit) : base(creatorEmail + boardName + ordinal, _tableName)
        {
            _creatorEmail = creatorEmail;
            _boardName = boardName;
            _ordinal = ordinal;
            _limit = limit;
        }

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
