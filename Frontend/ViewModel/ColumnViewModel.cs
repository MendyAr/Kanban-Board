using IntroSE.Kanban.Frontend.Commands;
using IntroSE.Kanban.Frontend.Model;
using System.Collections.ObjectModel;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class ColumnViewModel : NotifiableObject
    {

        private ColumnModel _column;
        private ObservableCollection<TaskModel> _tasks;
        private TaskModel _selectedTask;

        private string _message;
        private bool _enableForward;

        public ColumnModel Column { get => _column; set => _column = value; }

        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }

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
                _message = value;
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

        public AdvanceTaskCommand AdvanceTaskCommand { get; } = new AdvanceTaskCommand();

        // constructor

        public ColumnViewModel(ColumnModel column)
        {
            this._column = column;
            RefreshTasks();
        }

        //  methods

        public TaskModel GetSelectedTask()
        {
            return SelectedTask;
        }

        public void RefreshTasks()
        {
            Tasks = new ObservableCollection<TaskModel>();
            foreach (var task in Column.GetTasks())
                Tasks.Add(task);
        }
    }


}
