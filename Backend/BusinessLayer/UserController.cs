using System;
using System.Collections.Generic;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {
        private Dictionary<string,User> users;


        public UserController() // temprary constructor until we use DataBases
        {
            users = new Dictionary<string, User>();
        }

        // constructor when we will use data uploading
        //public UserController(List<DLUser> DLUsers) // ADTUser is a User from the DataLayer
        //{
        //    foreach (DLUser dataUser in DLUsers)
        //    {
        //        User user = new User(dataUser);
        //        this.users.Add(dataUser.email, user);
        //    }
        //}


        ///<summary>Registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="User">The User created by the registration.</returns>
        ///<exception cref="Exception">thrown when email is null, not in email structure or when user with this email already exist.</exception>
        internal User Register(string email, string password)
        {
            if (email == null)
                throw new Exception("Email cannot be null");

            if (!IsValidEmail(email))
            {
                throw new Exception("This email address is invalid, please check for spellMistakes");
            }

            if (EmailExist(email))
            {
                throw new Exception("A user already exist with this Email address");
            }

            User us = new User(email, password);
            this.users.Add(email, us);
            return us;
        }

        /// <summary>
        /// check if the input string match an email structure.
        /// </summary>
        /// <param name="email">the input email.</param>
        /// <returns>true/false accordingly.</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// check if a user with the same email address exist in the system.
        /// </summary>
        /// <param name="email">the input email need to be checked.</param>
        /// <returns>true/false accordingly.</returns>
        private bool EmailExist(string email)
        {
            if (email == null)
                return false;
            return users.ContainsKey(email);
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns cref="User">The User that logged in.</returns>
        /// <exception cref="Exception">thrown when a user with this email doesn't exist or when the password is incorrect.</exception>
        internal User Login(string email, string password)
        {
            if (EmailExist(email)) {
                User user = users[email];
                if (user.validatePassword(password)) 
                {
                    return user;
                }
                    
            }
            throw new Exception("Email or Password is invalid");
        }

       

    }
}
