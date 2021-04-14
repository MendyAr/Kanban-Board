using System;
using UC = IntroSE.Kanban.Backend.BuisnessLayer.UserController;
using BUser = IntroSE.Kanban.Backend.BuisnessLayer.User;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserService
    {
        //fields
        private UC uc = new UC();

        //constructor
        internal UserService()
        {
            uc = new UC();
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
            catch (Exception e)
            {
                return new Response(e.Message);
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
                BUser loginUser = uc.Login(email, password);
                User serviceLayerUser = new User(loginUser.Email);
                Response<User> response = Response<User>.FromValue(serviceLayerUser);
                return response;             
            }
            catch (Exception e)
            {
                return Response<User>.FromError(e.Message);
            }
        }

    }
}
