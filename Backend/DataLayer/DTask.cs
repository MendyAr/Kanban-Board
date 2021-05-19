using System;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DTask : DTO
    {
        // properties

        private readonly int taskId;
        private string title;
        private string description;
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private string assignee;
        private int _ordinal;
        private string _boardCreator;
        private string _boardName;

        private const string TITLE = "Title";
        private const string DESCRIPTION = "Description";
        private const string DUE_DATE = "DueDate";
        private const string ASSIGNEE = "Assignee";
        private const string ORDINAL = "Ordinal";
        private const string BOARD_CREATOR = "BoardCreator";
        private const string BOARD_NAME = "BoardName";

        internal int TaskId
        { get { return taskId; } }

        internal string Title
        {
            get { return title; }
            set
            {
                if (Persist)
                {
                    Update(TITLE, value);
                }
                title = value;
            }
        }

        internal string Description
        {
            get { return description; }
            set
            {

                if (Persist)
                {
                    Update(DESCRIPTION, value);
                }
                description = value;
            }
        }

        internal DateTime CreationTime { get { return creationTime; } }

        internal DateTime DueDate
        {
            get { return dueDate; }
            set
            {

                if (Persist)
                {
                    Update(DUE_DATE, value.ToString());
                }
                dueDate = value;
            }
        }

        internal string Assignee
        {
            get { return assignee; }
            set
            {
                if (Persist)
                {
                    Update(ASSIGNEE, value);
                }
                assignee = value;
            }
        }

        internal int Ordinal
        {
            get => _ordinal; set
            {
                if (Persist)
                {
                    Update(ORDINAL, value);
                }
                _ordinal = value;
            }
        }

        internal string BoardCreator
        {
            get => _boardCreator; set
            {
                if (Persist)
                {
                    Update(BOARD_CREATOR, value);
                }
                _boardCreator = value;
            }
        }

        internal string BoardName
        {
            get => _boardName; set
            {
                if (Persist)
                {
                    Update(BOARD_NAME, value);
                }
                _boardName = value;
            }
        }

        // constructor
        internal DTask(int taskId, DateTime creationTime, string title, string description, DateTime dueDate, string assignee, int ordinal, string boardCreator, string boardName) : base(boardCreator + boardName + taskId, "Task")
        {
            this.taskId = taskId;
            Title = title;
            Description = description;
            this.creationTime = creationTime;
            DueDate = dueDate;
            Assignee = assignee;
            Ordinal = ordinal;
            BoardCreator = boardCreator;
            BoardName = boardName;
        }

        // method
        protected override SQLiteCommand InsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO {_tableName}  VALUES (@{ID},@{TaskId}, @{CreationTime.ToString()}, @{Title}, @{Description}, @{DueDate.ToString()}, @{Assignee}, @{Ordinal}, {BoardCreator}, {BoardName})"
            };
            command.Parameters.Add(new SQLiteParameter(ID, ID));
            command.Parameters.Add(new SQLiteParameter(TaskId.ToString(),TaskId));
            command.Parameters.Add(new SQLiteParameter(CreationTime.ToString(), CreationTime));
            command.Parameters.Add(new SQLiteParameter(Title,Title));
            command.Parameters.Add(new SQLiteParameter(Description, Description));
            command.Parameters.Add(new SQLiteParameter(DueDate.ToString(), DueDate));
            command.Parameters.Add(new SQLiteParameter(Assignee, Assignee));
            command.Parameters.Add(new SQLiteParameter(Ordinal.ToString(), Ordinal));
            command.Parameters.Add(new SQLiteParameter(BoardCreator,BoardCreator));
            command.Parameters.Add(new SQLiteParameter(BoardName, BoardName));

            return command;
        }
    }
}
