using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class BoardViewModel : ViewModelObject
    {

        public BoardModel boardModel;
        public ObservableCollection<ColumnModel> columns;

        private string _boardName;
        private string _boardCreator;
        private string _message;
        private string _selectedMessage;
        private string _newColumnName;
        private string _newColumnOrdinal;

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

        public string SelectedMessage { get => _selectedMessage; set => _selectedMessage = value; }
        public string NewColumnName { get => _newColumnName; set => _newColumnName = value; }
        public string NewColumnOrdinal { get => _newColumnOrdinal; set => _newColumnOrdinal = value; }

        public BoardViewModel(BoardModel boardModel) : base(boardModel.Controller)
        {
            this.boardModel = boardModel;
            columns = boardModel.GetColumns();
            columns.CollectionChanged += HandleChange;
        }

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
                    //columns = boardModel.GetColumns();
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }
        }

        public void DeleteColumn()
        {

        }

        public ColumnModel GetColumn()
        {

        }

        public void RenameColumn() { }

        public void MoveColumn(int columnOrdinal, int shiftVal)
        {

        }

        public void LimitColumn(int limit)
        {

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ColumnModel c in e.OldItems)
                {

                    Controller.RemoveColumn(c);
                }

            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {

            }
        }
    }
}

