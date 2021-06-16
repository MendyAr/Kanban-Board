using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.Interfaces
{
    interface ILoginInstance
    {
        internal void Login(string userEmail);
        internal void ValidateLogin(string userEmail);
        internal void Logout(string userEmail);
    }
}
