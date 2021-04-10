using System;
using System.Collections.Generic;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {
        private Dictionary<string,User> users;


        public UserController() { } // an empty constructor while we dont upload data

        // constructor when we will use data uploading
        //public UserController(List<DLUser> DLUsers) // ADTUser is a User from the DataLayer
        //{
        //    foreach (DLUser dataUser in DLUsers)
        //    {
        //        User user = new User(dataUser);
        //        this.users.Add(dataUser.email, user);
        //    }
        //}


        public User Register(string email, string password)
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

        private bool EmailExist(string email)
        {
            return users.ContainsKey(email);
        }

        public User Login(string email, string password)
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
