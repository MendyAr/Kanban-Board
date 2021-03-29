using System;
using System.Collections.Generic;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {
        private Dictionary<string,User> users;
        public UserController(List<DataUser> ADTUsers) // ADTUser is a User from the DataLayer
        {
            foreach (DataUser dataUser in ADTUsers)
            {
                User user = new User(dataUser);
                this.users.Add(dataUser.email, user);
            }
        }
        public User Register(string email, int password)
        {
            if (EmailExist(email))
            {
                throw new Exception("A user already exist with this Email address");
            }

            return new User(email, password);
        }

        private bool EmailExist(string email)
        {
            bool emailExist = false;

           foreach(KeyValuePair<string,User> valuePair in users)
            {
                if (valuePair.Key.Equals(email))
                {
                    emailExist = true;
                    break;
                }
            }
            return emailExist;
        }

        public User Login(string email, int password)
        {
            if (EmailExist(email)) {
                User user = users[email];
                if (user.isMyPassword(password)) 
                {
                    return user;
                }
                    
            }
            throw new Exception("Email or Password is invalid");
        }

       

    }
}
