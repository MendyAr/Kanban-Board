using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IntroSE.Kanban.Frontend.Model
{
    public class User
    {
        private ObservableCollection<Board> _boards;

        public string Email { get; private set; }
        internal ObservableCollection<Board> Boards { get => _boards; }

        public User(string email, Collection<Board> boards)
        {
            this._boards = new ObservableCollection<Board>(boards);
            Email = email;
        }
        public void AddBoard(Board boardSignature)
        {
            _boards.Add(boardSignature);
        }
    }
}
