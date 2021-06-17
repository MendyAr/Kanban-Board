using System;
using System.Collections.Generic;
using SColumn = IntroSE.Kanban.Backend.ServiceLayer.Column;

namespace IntroSE.Kanban.Frontend.Model
{
    public class ColumnModel : NotifiableObject
    {
        private UserModel _user;
        private BoardModel _board;
        private List<TaskModel> _tasks;

        private string _name;
        private int _ordinal;
        private int _limit;

        public UserModel User { get => _user; }
        public BoardModel Board { get => _board; }

        public string Name
        {
            get => _name;
            set
            {
                try
                {
                    User.Controller.RenameColumn(User.Email, Board.CreatorEmail, Board.BoardName, Ordinal, value);
                    _name = value;
                    Board.Message = "Name changed successfully!";
                }
                catch (Exception e)
                {
                    Board.Message = "Cannot change name. " + e.Message;
                }
                RaisePropertyChanged("Name");
            }
        }
        public int Ordinal
        {
            get => _ordinal;
            set
            {
                try
                {
                    User.Controller.MoveColumn(User.Email, Board.CreatorEmail, Board.BoardName, _ordinal, value - _ordinal);
                    _ordinal = value;
                    Board.Message = "Ordinal changed successfully!";
                }
                catch (Exception e)
                {
                    Board.Message = "Cannot change ordinal. " + e.Message;
                }
                RaisePropertyChanged("Ordinal");
            }
        }

        public string Limit
        {
            get
            {
                if (_limit == -1)
                {
                    return "unlimited";
                }
                else
                {
                    return _limit.ToString();
                }
            }
            set
            {
                if (value == "unlimited")
                {
                    value = "-1";
                }
                try
                {
                    User.Controller.LimitColumn(User.Email, Board.CreatorEmail, Board.BoardName, Ordinal, int.Parse(value));
                    _limit = int.Parse(value);
                    Board.Message = "Task limit changed successfully!";
                }
                catch (Exception e)
                {
                    Board.Message = "Cannot change limit. " + e.Message;
                }
                RaisePropertyChanged("Limit");
            }
        }

        public List<TaskModel> Tasks { get => _tasks; set => _tasks = value; }

        public ColumnModel(BoardModel board, SColumn sColumn)
        {
            this._user = board.User;
            this._board = board;
            this._name = sColumn.Name;
            this._ordinal = sColumn.Ordinal;
            this._limit = sColumn.Limit;
            this._tasks = User.Controller.GetColumnTasks(this);

        }
    }
}
