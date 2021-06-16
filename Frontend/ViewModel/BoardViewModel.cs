using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class BoardViewModel : ViewModelObject
    {

        private BoardModel board;
        private ObservableCollection<ColumnModel> _columns;

        private string _message;
        private ColumnModel _selectedColumn;
        private string _newColumnName;
        private string _newColumnOrdinal;
        private bool _enableForward;

        public BoardModel Board { get => board; set => board = value; }
        public ObservableCollection<ColumnModel> Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                RaisePropertyChanged("Columns");
            }
        }

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
            this.board = boardModel;
            Columns = boardModel.GetColumns();
            Columns.CollectionChanged += HandleChange;
        }


        // methods
        public void AddColumn()
        {
            if (NewColumnName == "")
            {
                Message = "Enter a name please";
            }
            else
            {
                try
                {
                    Controller.AddColumn(Board, int.Parse(NewColumnOrdinal), NewColumnName);
                    Columns.Add(new ColumnModel(Board, NewColumnName, int.Parse(NewColumnOrdinal)));
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
                Controller.RemoveColumn(Board.User.Email, Board.CreatorEmail, Board.Name, SelectedColumn.Ordinal);
                Columns = Board.GetColumns();
                Message = "Column deleted successfully!";
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
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RaisePropertyChanged("Column");
            }
            

        }

    }
}

