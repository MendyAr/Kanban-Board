using System;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {   
        //filed
        
        private Dictionary<string,User> users;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        //constructor
        
        public UserController() // temprary constructor until we use DataBases
        {
            users = new Dictionary<string, User>();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
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


        //methods

        ///<summary>Registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="User">The User created by the registration.</returns>
        ///<exception cref="Exception">thrown when email is null, not in email structure or when user with this email already exist.</exception>
        internal User Register(string email, string password)
        {
            if (email == null)
            {
                log.Info("Tryng to Register with null email!");
                throw new Exception("Email cannot be null");
            }
            if (!IsValidEmail(email))
            {
                log.Info("Trying to register with invalid email!");
                throw new Exception("This email address is invalid, please check for spellMistakes");
            }

            if (EmailExist(email))
            {
                log.Info("Trying to register with an existing email!");
                throw new Exception("A user already exist with this Email address");
            }

            User us = new User(email, password);
            this.users.Add(email, us);
            log.Info("new User created and added successfully!");
            return us;
        }

        /// <summary>
        /// check if the input string match an email structure.
        /// </summary>
        /// <param name="email">the input email.</param>
        /// <returns>true/false accordingly.</returns>
        private bool IsValidEmail(string email)
        {
            var emailValidator = new EmailAddressAttribute();
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email &emailValidator.IsValid(email) ;
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
                    log.Info("login successfully!");
                    return user;
                }
                    
            }
            log.Info("Failed to login!");
            throw new Exception("Email or Password is invalid");
        }

       

    }
}
