﻿using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IntroSE.Kanban.Frontend.ViewModel
{

    class UserViewModel : ViewModelObject
    {
        private UserModel _user;
        private string _message;
        private BoardModel _selectedBoard;
        private string _joinBoardCreator;
        private string _joinBoardName;
        private string _newBoardName;
        private bool _isJoinArgumentProvide = false;
        private bool _isBoardlSelected = false;
        private bool _isCreatBoardArgumentProvide = false;
        private ObservableCollection<BoardModel> _boards;
        public UserModel User { get => _user; private set { _user = value; } }
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

        

        public BoardModel SelectedBoard { 
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
                _isJoinArgumentProvide = (JoinBoardName != "" & JoinBoardCreator != "") & (JoinBoardName != null & JoinBoardCreator != null);
                RaisePropertyChanged("IsJoinArgumentProvide");
            }
        }

        public bool IsCreateBoardArgumentProvide
        {
            get => _isCreatBoardArgumentProvide;
            set
            {
                _isCreatBoardArgumentProvide = NewBoardName != "" & NewBoardName != null;
                RaisePropertyChanged("IsCreatBoardArgumentProvide");
            }
        }
        public string NewBoardName
        {
            get => _newBoardName;
            set
            {
                _newBoardName = value;
                IsCreateBoardArgumentProvide = false; // does not matter what value
            }
        }


        public string Message { get=>_message; 
            set { _message= value;
                RaisePropertyChanged("Message"); } }
   

        internal void Join_Board()
        {
            Message = "";
            try
            {
                BoardModel joindBoard = Controller.JoinBoard(User, JoinBoardCreator, JoinBoardName);
                Message = "You Joined the board " + JoinBoardName + " Successfully";
                Boards.Add(joindBoard);
            }
            catch(Exception e)
            {
                Message = "You Failed to join Board " + JoinBoardName + " because " + e.Message;
            }
        }

        internal void CreateBoard()
        {
            Message = "";
            try
            {
                BoardModel board = Controller.AddBoard(User, NewBoardName);
                Message = $"You Created the board {NewBoardName} successfully";
                Boards.Add(board);
            }
            catch (Exception e)
            {
                Message = "You Failed to create Board " + NewBoardName + " because " + e.Message;
            }
        }
        internal BoardModel OpenBoard()
        {

            return SelectedBoard;
        }

        public  IList<TaskModel> GetInProggress()
        {
            IList<TaskModel> tasks= Controller.GetInProgressTasks(User.Email,Controller);

            return tasks;
        }

        public ObservableCollection<BoardModel> Boards { get => _boards; private set => _boards=value; }

        public UserViewModel(UserModel user,BackendController backendController):base(backendController)
        {
            User = user;
            Boards = User.GetBoards();
            Boards.CollectionChanged += HandleBoardsChange;
        }

        private void HandleBoardsChange(object sender, NotifyCollectionChangedEventArgs e)
        {
           /* //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (MessageModel y in e.OldItems)
                {

                    Controller.RemoveMessage(user.Email, y.Id);
                }

            }
           */

            RaisePropertyChanged("Boards");
        }

    }
}
