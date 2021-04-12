using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class User
    {

        private readonly string email;
        private string password;
        private static readonly int PASS_MIN_LENGTH = 4;
        private static readonly int PASS_MAX_LENGTH = 20;

        public string Email { get { return email; } }
        private string Password
        {
            get { return password; }
            set
            {
                validatePasswordRules(value);
                password = value;
            }
        }


        internal User(string email, string pass)
        {
            if (email == null)
                throw new ArgumentNullException("email can't be null");
            if (pass == null)
                throw new ArgumentNullException("password can't be null");
            
            this.email = email;
            this.Password = pass;
        }

        /// <summary>validate password with the user saved password.</summary>
        /// <param name="pass">the password given for check.</param>
        /// <returns>true/false accordingly.</returns>
        internal bool validatePassword(string pass)
        {
            return Password.Equals(pass);
        }
        /// <summary>
        /// check the structure of the password.
        /// </summary>
        /// <param name="pass">password to check.</param>
        /// <exception cref="Exception">thrown when the password doesn't apply the structure rules.</exception>
        private void validatePasswordRules(string pass)
        {
            if (pass.Length > PASS_MAX_LENGTH | pass.Length < PASS_MIN_LENGTH) //check length
                throw new Exception("password length is incorrect");
            
            char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            if (pass.IndexOfAny(numbers) == -1)         // check contain a number
                throw new Exception("the password doesn't contain a number");

            char[] lowerCase = { 'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                'n','o','p','q','r','s','t','u','v','w','x','y','z'};
            if (pass.IndexOfAny(lowerCase) == -1)       // check contain a lower case letter
                throw new Exception("the password doesn't contain a lowercase letter");

            char[] upperCase = { 'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            if (pass.IndexOfAny(upperCase) == -1)       // check contain an upper case letter
                throw new Exception("the password doesn't contain a upper case letter");
        }
    }
}
