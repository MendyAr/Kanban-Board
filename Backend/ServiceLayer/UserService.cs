using System.Collections.Generic;
using System;
using System.Linq;

using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        //fields
        private UserController uc = new UserController();

        //constructor
        internal UserService()
        {
            uc = new UserController();
        }

        //functions

        ///<summary>Registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>


        internal Response Register(string email, string password)
        {
            try
            {
                uc.Register(email, password);
                return new Response();
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }
        
        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        internal Response<User> Login(string email, string password)
        {
            try
            {
                BuisnessLayer.User  loginUser = uc.Login(email, password);
                User serviceLayerUser = new User(loginUser.Email);
                Response<User> re = Response<User>.FromValue(serviceLayerUser);
                return re;             
            }
            catch (Exception er)
            {
                return Response<User>.FromError(er.Message);
            }
        }

    }
}
