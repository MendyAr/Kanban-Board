using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {

        private BoardModel board;
        private List<ColumnModel> _column;
        private ColumnModel _selectedColumn;

        private string _newColumnName;
        private string _newColumnOrdinal;
        private string _message;
        private bool _enableForward;

        public BoardModel Board { get => board; }
        public string BoardName { get => Board.BoardName; }
        public string CreatorEmail { get => Board.CreatorEmail; }

        public List<ColumnModel> Columns
        {
            get => _column;
            set
            {
                _column = value;
                RaisePropertyChanged("Columns");
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

        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

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
        public BoardViewModel(BoardModel boardModel)
        {
            this.board = boardModel;
            RefreshColumns();
        }


        // methods

        public void RefreshColumns()
        {
            List<ColumnModel> newColumns = new List<ColumnModel>();
            foreach (var column in Board.GetColumn())
                newColumns.Add(column);
            Columns = newColumns;
        }

        public ColumnModel GetSelectedColumn()
        {
            return SelectedColumn;
        }

        public void AddColumn()
        {
            if (NewColumnName == "")
            {
                Message = "Enter a name please";
            }
            else if (!int.TryParse(NewColumnOrdinal, out int result))
            {
                Message = "Enter a number please";
            }
            else
            {
                try
                {
                    Board.AddColumn(result, NewColumnName);
                    RefreshColumns();
                    Message = "Column added successfully!";
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
                Board.DeleteColumn(SelectedColumn.ordinal);
                RefreshColumns();
                Message = "Column deleted successfully!";
            }
            catch (Exception e)
            {
                Message = "Failed to delete column. " + e.Message;
            }
        }

    }
}

