using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBoard = IntroSE.Kanban.Backend.ServiceLayer.Board;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private UserModel _user;
        private string _creatorEmail;
        private string _name;
        private int _columnCount;
        private int _taskCount;

        public UserModel User { get => _user; }
        public string CreatorEmail
        {
            get => _creatorEmail;
            set
            {
                _creatorEmail = value;
                RaisePropertyChanged("BoardCreator");
            }
        }
        public string Name
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
                RaisePropertyChanged("BoardColumnCount");
            }
        }
        public int TaskCount
        {
            get => _taskCount;
            set
            {
                _taskCount = value;
                RaisePropertyChanged("BoardTaskCount");
            }
        }

        public BoardModel(UserModel user, SBoard sBoard) : base(user.Controller)
        {
            this._user = user;
            this.CreatorEmail = sBoard.CreatorEmail;
            this.Name = sBoard.Name;
            this.ColumnCount = sBoard.ColumnCount;
            this.TaskCount = sBoard.TaskCount;
        }

        public ObservableCollection<ColumnModel> GetColumns()
        {
            return Controller.GetBoardColumns(this);
        }
    }
}
