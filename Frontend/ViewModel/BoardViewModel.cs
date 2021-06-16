using IntroSE.Kanban.Frontend.Commands;
using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class BoardViewModel : ViewModelObject
    {

        private BoardModel board;
        private ObservableCollection<ColumnModel> _columns;

        private string _message;
        private ColumnModel _selectedColumn;
        private string _newColumnName;
        private string _newColumnOrdinal;
        private bool _enableForward;

        public BoardModel Board { get => board; set => board = value; }

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

        public AddColumnCommand AddColumnCommand { get; } = new AddColumnCommand();
        public DeleteColumnCommand DeleteColumnCommand { get; } = new DeleteColumnCommand();

        // constructor
        public BoardViewModel(BoardModel boardModel) : base(boardModel.Controller)
        {
            this.board = boardModel;
        }


        // methods

        public ColumnModel GetColumn()
        {
            return SelectedColumn;
        }

    }
}

