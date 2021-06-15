using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SColumn = IntroSE.Kanban.Backend.ServiceLayer.Column;

namespace IntroSE.Kanban.Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        private UserModel _user;
        private BoardModel _board;
        private int _ordinal;
        private string _name;
        private int _limit;

        public UserModel User { get => _user; }
        public BoardModel Board { get => _board; }
        public int Ordinal { get => _ordinal; 
            set
            {
                _ordinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        public string Name { get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("ColumnName");
            }
        }
        public int Limit { get => _limit; 
            set
            {
                _limit = value;
                RaisePropertyChanged("ColumnLimit");
            }
        }

        public ColumnModel(BoardModel board, SColumn sColumn) : base(board.Controller)
        {
            this._user = board.User;
            this._board = board;
            this.Ordinal = sColumn.Ordinal;
            this.Name = sColumn.Name;
            this.Limit = sColumn.Limit;
        }

        public ObservableCollection<TaskModel> GetTasks()
        {
            return Controller.GetColumnTasks(this);
        }
    }
}
