using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class BoardViewModel : ViewModelObject
    {

        private BoardModel board;

        private string _boardName;
        private string _boardCreator;
        private string _message;

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

        public BoardViewModel(BoardModel boardModel)
        {
            this.Controller = boardModel.Controller;
        }

        //RollBack?

        //scroll

        public void AddColumn() { }

        public void MoveColumn(int columnOrdinal, int shiftVal)
        {

        }

        public void LimitColumn(int limit)
        {

        }

        public void RenameColumn() { }

        public void DeleteColumn(String columnName) { }

        public void AddTask() { }




    }
}
