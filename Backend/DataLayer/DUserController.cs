using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DUserController : DalController
    {

        internal DUserController() : base("User")
        { }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            string email = reader.GetString(1);
            string password = reader.GetString(2);
            return new DUser(email, password);
        }
    }
}



