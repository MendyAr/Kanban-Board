using System;
using System.Collections.Generic;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {
        private Dictionary<string,User> users;

        public UserController()
        {

        }
        /*public UserController(List<DataUser> du)
        {
            foreach (DataUser dataUser in du)
            {
                User user = new User(dataUser.Email, dataUser.Password); //possible to add builder in User
                this.users.Add(user.Email, user);
            }
        }*/
        public User Register(string email, string password)
        {
            if (email == null)
                throw new Exception("Email can't ne null");

            if (EmailExist(email))
                throw new Exception("A user already exist with this Email address");

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
