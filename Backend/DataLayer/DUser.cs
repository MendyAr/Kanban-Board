using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DUser : DTO
    {
        // properties
        private readonly string email;
        private string password;

        internal string Email { get { return email; } }
        internal string Password { get { return password; }}

        // constructor
        internal DUser(string email, string password) : base (new DUserController(), email)
        { 
            this.email = email;
            this.password = password;
            Insert();
        }   

    }
}
