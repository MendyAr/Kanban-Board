using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                try
                {
                    Controller.MoveColumn(User.Email, Board.CreatorEmail, Board.Name, _ordinal, value - _ordinal);
                    _ordinal = value;
                }
                catch(Exception e)
                {
                    MessageBox.Show("Cannot change ordinal. " + e.Message);
                }
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        public string Name { get => _name;
            set
            {
                try
                {
                    Controller.RenameColumn(User.Email, Board.CreatorEmail, Board.Name, Ordinal, value);
                    _name = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot change name. " + e.Message);
                }
                RaisePropertyChanged("ColumnName");
            }
        }
        public int Limit { get => _limit; 
            set
            {
                try
                {
                    Controller.LimitColumn(User.Email, Board.CreatorEmail, Board.Name, Ordinal, value);
                    _limit = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot change limit. " + e.Message);
                }
                RaisePropertyChanged("ColumnLimit");
            }
        }

        public ColumnModel(BoardModel board, SColumn sColumn) : base(board.Controller)
        {
            this._user = board.User;
            this._board = board;
            this._ordinal = sColumn.Ordinal;
            this._name = sColumn.Name;
            this._limit = sColumn.Limit;
        }

        public ObservableCollection<TaskModel> GetTasks()
        {
            return Controller.GetColumnTasks(this);
        }
    }
}
