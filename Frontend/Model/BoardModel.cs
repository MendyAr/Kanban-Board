using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBoard = IntroSE.Kanban.Backend.ServiceLayer.Board;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private UserModel _user;
        private ObservableCollection<ColumnModel> columns;

        private string _creatorEmail;
        private string _name;
        private int _columnCount;
        private int _taskCount;

        public string FullName { get => CreatorEmail + " : " + BoardName; }

        public UserModel User { get => _user; }
        
        public ObservableCollection<ColumnModel> Columns
        {
            get => columns;
            set
            {
                columns = value;
                RaisePropertyChanged("Columns");
            }
        }
        public string CreatorEmail
        {
            get => _creatorEmail;
            set
            {
                _creatorEmail = value;
                RaisePropertyChanged("CreatorEmail");
            }
        }
        public string BoardName
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("BoardName");
            }
        }
        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                _columnCount = value;
                RaisePropertyChanged("ColumnCount");
            }
        }
        public int TaskCount
        {
            get => _taskCount;
            set
            {
                _taskCount = value;
                RaisePropertyChanged("TaskCount");
            }
        }

        public BoardModel(UserModel user, SBoard sBoard) : base(user.Controller)
        {
            this._user = user;
            this._creatorEmail = sBoard.CreatorEmail;
            this._name = sBoard.Name;
            this._columnCount = sBoard.ColumnCount;
            this._taskCount = sBoard.TaskCount;
            Columns = Controller.GetBoardColumns(this);
            Columns.CollectionChanged += HandleChange;
        }

        public void AddColumn(int newColumnOrdinal, string newColumnName)
        {
            Controller.AddColumn(User.Email, CreatorEmail, BoardName, newColumnOrdinal, newColumnName);
            ColumnCount++;
            Columns = Controller.GetBoardColumns(this);
            RaisePropertyChanged("Column");
        }

        public void DeleteColumn(int columnOrdinal)
        {
            Controller.RemoveColumn(User.Email, CreatorEmail, BoardName, columnOrdinal);
            ColumnCount = ColumnCount - 1;
            Columns = Controller.GetBoardColumns(this);
            RaisePropertyChanged("Column");
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
