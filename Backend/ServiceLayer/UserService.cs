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
