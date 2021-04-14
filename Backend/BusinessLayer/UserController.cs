using System;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


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

        //methods

        ///<summary>Registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="User">The User created by the registration.</returns>
        ///<exception cref="Exception">thrown when email is null, not in email structure or when user with this email already exist.</exception>
        internal User Register(string email, string password)
        {
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
        ///<exception cref="Exception">thrown when email is null or not in email structure.</exception>
        private void ValidateEmail(string email)
        {
            if (email == null)
            {
                log.Info("Tryng to Register with null email!");
                throw new Exception("Email cannot be null");
            }

            var emailValidator = new EmailAddressAttribute();
            Regex rg = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            bool validEmail = rg.IsMatch(email);
            char[] invalidCharacters = { 'א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט', 'י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ', 'ק', 'ר', 'ש', 'ת', 'ך', 'ם', 'ן', 'ף', 'ץ', '+', '*', '/', '`', '~' };
            var addr = new System.Net.Mail.MailAddress(email);
            if(!(addr.Address == email & emailValidator.IsValid(email) & validEmail & email.IndexOfAny(invalidCharacters) == -1))
            {
                log.Info("Trying to register with invalid email!");
                throw new Exception("This email address is invalid, please check for spellMistakes");
            }
        }

        /// <summary>
        /// check if a user with the same email address exist in the system.
        /// </summary>
        /// <param name="email">the input email need to be checked.</param>
        /// <returns>true/false accordingly.</returns>
        /// ///<exception cref="Exception">thrown when email is null or not in email structure.</exception>
        internal bool EmailExist(string email)
        {
            ValidateEmail(email); 
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
