using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DUser : DTO
    {
        // properties
        private readonly string email;
        private string password;

        internal string Email { get { return email; } }
        internal string Password { get { return password; } }

        // constructor
        internal DUser(string email, string password) : base(email, "User")
        {
            this.email = email;
            this.password = password;
            Insert();
            Persist = true;
        }

        protected override SQLiteCommand InsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO {_tableName}  VALUES (@{ID}, @{Email}, @{Password})"
            };
            command.Parameters.Add(new SQLiteParameter(ID, ID));
            command.Parameters.Add(new SQLiteParameter(Email, Email));
            command.Parameters.Add(new SQLiteParameter(Password, Password));

            return command;
        }
    }
}

