using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class User
    {
        ObservableCollection<Board> boards;
        public User(IntroSE.Kanban.Backend.ServiceLayer.User value)
        {
           
        }

        public string Email { get; internal set; }
    }
}
