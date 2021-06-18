using IntroSE.Kanban.Frontend.Model;
using System;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class AddingNewTaskViewModel : ViewModelObject
    {
        private string _message;
        private BoardModel _board;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        public BoardModel Board { get => _board; }

        public AddingNewTaskViewModel(BoardModel board) : base(board.User.Controller)
        {
            this._board = board;
        }

        internal void AddTask()
        {
            try
            {
                Message = "";
                TaskModel task = Controller.AddTask(_board, Title, Description, DueDate);
                Board.Columns[0].Tasks.Add(task);
                Message = $" The Task '{Title}' had been created successfully";
            }
            catch (Exception e)
            {
                Message = $"Fail to create task because {e.Message}";
            }
        }
    }
}
