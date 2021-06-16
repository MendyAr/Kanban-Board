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
                    Controller.MoveColumn(User.Email, Board.CreatorEmail, Board.BoardName, _ordinal, value - _ordinal);
                    _ordinal = value;
                    MessageBox.Show("Ordinal changed successfully!");
                }
                catch(Exception e)
                {
                    MessageBox.Show("Cannot change ordinal. " + e.Message);
                }
                RaisePropertyChanged("Ordinal");
            }
        }
        public string Name { get => _name;
            set
            {
                try
                {
                    Controller.RenameColumn(User.Email, Board.CreatorEmail, Board.BoardName, Ordinal, value);
                    _name = value;
                    MessageBox.Show("Name changed successfully!");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot change name. " + e.Message);
                }
                RaisePropertyChanged("Name");
            }
        }
        public int Limit { get => _limit; 
            set
            {
                try
                {
                    Controller.LimitColumn(User.Email, Board.CreatorEmail, Board.BoardName, Ordinal, value);
                    _limit = value;
                    MessageBox.Show("Task limit changed successfully!");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot change limit. " + e.Message);
                }
                RaisePropertyChanged("Limit");
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

        public ColumnModel(BoardModel board, string Name, int Ordinal) : base(board.Controller)
        {
            this._user = board.User;
            this._board = board;
            this._name = Name;
            this._ordinal = Ordinal;
            this._limit = -1;
        }

        public ObservableCollection<TaskModel> GetTasks()
        {
            return Controller.GetColumnTasks(this);
        }
    }
}
