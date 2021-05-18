using DUser = IntroSE.Kanban.Backend.DataLayer.DUser;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        //fields

        private readonly string email;
        private string password;

        internal string Email { get => email; }
        private string Password { get => password; }

        //constructor

        internal User(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        //constructor for Duser objects
        internal User(DUser dUser)
        {
            this.email = dUser.Email;
            this.password = dUser.Password;
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
