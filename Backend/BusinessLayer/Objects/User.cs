using DUser = IntroSE.Kanban.Backend.DataLayer.DUser;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        //fields

        private readonly string email;
        private string password;
        DUser dUser;

        internal string Email { get => email; }
        private string Password { get => password; }


        //constructor

        internal User(string email, string password)
        {
            this.email = email;
            this.password = password;
            dUser = new DUser(Email, Password);
            dUser.Insert();
            dUser.Persist = true;
        }

        //constructor for Duser objects
        internal User(DUser dUser)
        {
            email = dUser.Email;
            password = dUser.Password;
            this.dUser = dUser;
            this.dUser.Persist = true;

        }

        //methods

        /// <summary>validate password with the user saved password.</summary>
        /// <param name="password">the password given for check.</param>
        /// <returns>true/false accordingly.</returns>
        internal bool validatePassword(string password)
        {
            return this.Password.Equals(password);
        }
    }
}



