using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal class DUserController : DalController <DUser>
    {

        internal DUserController() : base("User")
        { }

        protected override DUser ConvertReaderToObject(SQLiteDataReader reader)
        {
            string email = reader.GetString(1);
            string password = reader.GetString(2);
            return new DUser(email, password);
        }
    }
}



