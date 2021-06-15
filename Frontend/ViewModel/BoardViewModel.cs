﻿using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class BoardViewModel : ViewModelObject
    {

        public BoardModel boardModel;
        public ObservableCollection<ColumnModel> columns;

        private string _boardName;
        private string _boardCreator;
        private string _message;
        private ColumnModel _selectedColumn;
        private string _newColumnName;
        private string _newColumnOrdinal;
        private bool _enableForward;

        public string BoardName { get => _boardName; set => _boardName = value; }

        public string BoardCreator { get => _boardCreator; set => _boardCreator = value; }

        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public ColumnModel SelectedColumn 
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
        }

        public string NewColumnName { get => _newColumnName; set => _newColumnName = value; }
        public string NewColumnOrdinal { get => _newColumnOrdinal; set => _newColumnOrdinal = value; }
        public bool EnableForward 
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        // constructor
        public BoardViewModel(BoardModel boardModel) : base(boardModel.Controller)
        {
            this.boardModel = boardModel;
            columns = boardModel.GetColumns();
            columns.CollectionChanged += HandleChange;
        }


        // methods
        public void AddColumn()
        {
            if (NewColumnName == null)
            {
                Message = "Enter a name please";
            }
            else if (NewColumnOrdinal == null)
            {
                Message = "Enter an ordinal please";
            }
            else
            {
                try
                {
                    Controller.AddColumn(boardModel, int.Parse(NewColumnOrdinal), NewColumnName);
                    columns = boardModel.GetColumns();
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }
        }

        public void DeleteColumn()
        {
            try
            {
                Controller.RemoveColumn(boardModel.User.Email, boardModel.CreatorEmail, boardModel.Name, SelectedColumn.Ordinal);
                columns = boardModel.GetColumns(); //? columns.remove(selectedColumn), but it can affect the whole set
            }
            catch(Exception e)
            {
                Message = e.Message;
            }
        }

        public ColumnModel GetColumn()
        {
            return SelectedColumn;
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("columns");
        }
    }
}

