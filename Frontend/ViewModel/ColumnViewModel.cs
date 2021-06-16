using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class ColumnViewModel : ViewModelObject
    {

        private ColumnModel _column;
        private ObservableCollection<TaskViewModel> _tasks;
        
        private TaskModel _selectedTask;

        private bool _enableForward;
        private string _message;
        private string _filter;

        internal ColumnModel Column { get => _column; set => _column = value; }
        internal ObservableCollection<TaskViewModel> Tasks { get => _tasks; set => _tasks = value; }

        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                EnableForward = value != null;
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                FilterContent();
            }
        }

    public ColumnViewModel(ColumnModel column) : base(column.Controller)
        {
            this.Column = column;
            //Tasks = Column.GetTasks();
        }

        public void AdvanceTask()
        {

        }

        public TaskModel getSelectedTask()
        {
            return SelectedTask;
        }

        private void FilterContent() 
        { 
        
        }
    }


}
