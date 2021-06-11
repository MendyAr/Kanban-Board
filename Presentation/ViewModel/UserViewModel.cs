using Frontend.Model;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{

    class UserViewModel : ViewModelObject
    {
        private User _user;
        public User User { get => _user; private set { _user = value; } }
        public string Title { get => "Boards for " + User.Email; }
        public bool IsBoardSelected;
        public Board SelectedBoard { get; set; }
        public string RegisterCreator;
        public string RegisterBoardName;
        public bool IsRegisterArgumentProvide;
        public bool IsCreatBoardArgumentProvide;
        public string NewBoardName;
        public string ErrorMessage;
        

        public UserViewModel(User user) 
        {
            User = user;
        }

        internal void Join_Board()
        {
            try
            {
                backendController.JoinBoard
            }
        }
    }
}
