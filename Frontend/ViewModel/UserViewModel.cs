using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{

    class UserViewModel : ViewModelObject
    {
        private User _user;
        private string _message;
        private bool _isBoardlSelected= false;
        private string _selectedBoard;
        private string _joinBoardCreator;
        private string _joinBoardName;
        private string _newBoardName;
        private bool _isJoinArgumentProvide = false;
        private bool _isCreatBoardArgumentProvide = false;
        public User User { get => _user; private set { _user = value; } }
        public string Title { get => "Boards for " + User.Email; }
        public bool IsBoardSelected
        {
            get => _isBoardlSelected;
            set 
            {
                _isBoardlSelected = SelectedBoard != null;
                RaisePropertyChanged("IsBoardSelected");
            }
        }

        

        public string SelectedBoard { 
            get=>_selectedBoard; 
            set 
            {
                _selectedBoard = value;
                IsBoardSelected = false; // does not matter what value
            } 
        }

        public string JoinBoardCreator 
        { 
            get=>_joinBoardCreator;
            set 
            {
                _joinBoardCreator = value;
                IsJoinArgumentProvide = false; // does not matter what value
            }
        }

    

        public string JoinBoardName
        {
            get => _joinBoardName;
            set
            {
                _joinBoardName = value;
                IsJoinArgumentProvide = false; // does not matter what value
            }
        }
        public bool IsJoinArgumentProvide 
        {
            get => _isJoinArgumentProvide;
            set
            {
                _isJoinArgumentProvide = JoinBoardName != "" & JoinBoardCreator != "";
                RaisePropertyChanged("IsJoinArgumentProvide");
            }
        }

        public bool IsCreatBoardArgumentProvide
        {
            get => _isCreatBoardArgumentProvide;
            set
            {
                _isCreatBoardArgumentProvide = NewBoardName != "";
                RaisePropertyChanged("IsCreatBoardArgumentProvide");
            }
        }
        public string NewBoardName
        {
            get => _newBoardName;
            set
            {
                _newBoardName = value;
                IsCreatBoardArgumentProvide = false; // does not matter what value
            }
        }
        public string Message { get=>_message; 
            set { _message= value;
                RaisePropertyChanged("Message"); } }
        

        public UserViewModel(User user) 
        {
            User = user;
           // does not matter that value
        } 

        internal void Join_Board()
        {
            Message = "";
            try
            {
                backendController.JoinBoard(User.Email, JoinBoardCreator, JoinBoardName);
                Message = "You Joined the board " + JoinBoardName + " Successfully";
                User.JoinBoard(JoinBoardCreator, JoinBoardName);
            }
            catch(Exception e)
            {
                Message = "You Failed to join Board " + JoinBoardName + " because " + e.Message;
            }
        }

        internal void createBoard()
        {
            Message = "";
            try
            {
                backendController.AddBoard(User.Email, NewBoardName);
                User.addBoard(NewBoardName);
            }
            catch (Exception e)
            {
                Message = "You Failed to create Board " + NewBoardName + " because " + e.Message;
            }
        }
        internal Board OpenBoard()
        {
            
            return backendController.loadBoard(SelectedBoard);
        }
    }
}
