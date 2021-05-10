using System;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataLayer.DUserController;
using IntroSE.Kanban.Backend.DataLayer.DUser;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {
        //fields

        private Dictionary<string, User> users; //key - email, value - user of that email
        private DUserController DUC;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //password limiters
        private static readonly int PASS_MIN_LENGTH = 4;
        private static readonly int PASS_MAX_LENGTH = 20;

        //constructors
        public UserController()
        {
            users = new Dictionary<string, User>();
            this.DUC = new DUserController();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        //methods

        ///<summary>This method loads the users data from the persistance </summary>
        internal void LoadData()
        {
            try
            {
                List<DUser> Dusers = DUserController.LoadData();
                foreach (DUser Duser in Dusers)
                {
                    User user = new User(Duser);
                    users[user.Email] = user;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        ///<summary>Removes all persistent users data.</summary>
        internal void DeleteData()
        {
            try
            {
                DUserController.DeleteData();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }




        ///<summary>Registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="User">The User created by the registration.</returns>
        ///<exception cref="Exception">thrown when email is null, not in email structure or when user with this email already exist.</exception>
        internal void Register(string email, string password)
        {
            try
            {
                if (users.ContainsKey(email))
                {
                    log.Warn("FAILED register attempt: '" + email + "' already exists");
                    throw new Exception("A user already exist with this Email address");
                }

                ValidateEmail(email);
                validatePasswordRules(password);
            }
            catch (ArgumentNullException)
            {
                log.Info("FAILED register attempt: '" + "null email " + "' an email cannot be null" );
                throw new Exception("an email cannot be null");
            }
            catch (Exception e)
            {
                log.Info("FAILED register attempt: '" + email + "' " + e.Message);
                throw new Exception(e.Message);
            }
            User newUser = new User(email, password);
            this.users.Add(email, newUser);
            log.Info("SUCCESSFULLY registered attempt: '" + email + "'");
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
            if (users.ContainsKey(email))
            {
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

        /// <summary>
        /// check if the input string match an email structure.
        /// </summary>
        /// <param name="email">the input email.</param>
        ///<exception cref="Exception">thrown when email is null or not in email structure.</exception>
        private void ValidateEmail(string email)
        {
            if (email == null)
            {
                throw new Exception("Email cannot be null");
            }

            var emailValidator = new EmailAddressAttribute();
            Regex rg = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            bool validEmail = rg.IsMatch(email);
            var addr = new System.Net.Mail.MailAddress(email);
            if (!(addr.Address == email & emailValidator.IsValid(email) & validEmail))
            {
                throw new Exception("This email address is invalid, please check for spellMistakes");
            }
        }

        /// <summary>
        /// check the structure of the password.
        /// </summary>
        /// <param name="pass">password to check.</param>
        /// <exception cref="Exception">thrown when the password doesn't apply the structure rules.</exception>
        private void validatePasswordRules(string pass)
        {
            if (pass == null)   //check null input
                throw new Exception("password is null");

            //check length
            if (pass.Length < PASS_MIN_LENGTH)
                throw new Exception("password too short");
            if (pass.Length > PASS_MAX_LENGTH)
                throw new Exception("password too long");


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