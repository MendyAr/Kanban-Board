using IntroSE.Kanban.Frontend.Model;
using System;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class AddingNewTaskViewModel : ViewModelObject
    {
        private string _message;
        private ColumnModel _column;
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

        public ColumnModel Column { get => _column; }

        public AddingNewTaskViewModel(ColumnModel column) : base(column.Controller)
        {
            this._column = column;
        }

        internal void AddTask()
        {
            try
            {
                Message = "";
                TaskModel newTask = Controller.AddTask(_column, Title, Description, DueDate);
                Message = $" The Task '{Title}' had been created successfully";
            }
            catch(Exception e)
            {
                Message = $"Fail to create task because {e.Message}";
            }
        }
    }
}
