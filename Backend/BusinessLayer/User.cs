using System;
using IntroSE.Kanban.Backend.DataLayer.DUser;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class User
    {
        //fields

        private readonly string email;
        private string password;

        public string Email { get { return email; } }
        private string Password { get; set; }

        //constructor

        public User(string email, string pass)
        {
            if (email == null)
                throw new ArgumentNullException("email can't be null");
            if (pass == null)
                throw new ArgumentNullException("password can't be null");
            
            this.email = email;
            this.Password = pass;
        }

        //constructor for Duser objects
        public User(DUser)
        {
            this.email = DUser.Email;
            this.Password = DUser.Password;
        }

        //methods

        /// <summary>validate password with the user saved password.</summary>
        /// <param name="pass">the password given for check.</param>
        /// <returns>true/false accordingly.</returns>
        internal bool validatePassword(string pass)
        {
            return Password.Equals(pass);
        }
    }
}
