using System.Collections.Generic;
using System;
using System.Linq;

using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private UserController uc = new UserController();
        internal Response Register(string email, string password)
        {
            throw new NotImplementedException();
        }

        internal Response<User> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        private void ValidateUserLoggin(string email)
        {
            throw new NotImplementedException();
        }

    }
}
