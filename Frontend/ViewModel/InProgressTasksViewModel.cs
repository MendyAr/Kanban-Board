using IntroSE.Kanban.Frontend.Model;
using System.Collections.Generic;


namespace IntroSE.Kanban.Frontend.ViewModel
{
    class InProgressTasksViewModel :ViewModelObject
    {
        IList<TaskModel> _tasks;
        TaskModel _selectedTask;

        public string Title { get => "In progress Tasks"; }
        private string ID 
        { 
            get 
            {
                if(SelectedTask == null)
                {
                    return "";
                }
                else
                {
                    return SelectedTask.ID.ToString();
                }
            }
        }

        private string CreationTime
        {
            get
            {
                if (SelectedTask == null)
                {
                    return "";
                }
                else
                {
                    return SelectedTask.CreationTime.ToString();
                }
            }
        }

        private string Description
        {
            get
            {
                if (SelectedTask == null)
                {
                    return "";
                }
                else
                {
                    return SelectedTask.Description.ToString();
                }
            }
        }

        private string DueDate
        {
            get
            {
                if (SelectedTask == null)
                {
                    return "";
                }
                else
                {
                    return SelectedTask.DueDate.ToString();
                }
            }
        }

        private string EmailAssignee
        {
            get
            {
                if (SelectedTask == null)
                {
                    return "";
                }
                else
                {
                    return SelectedTask.Assignee.ToString();
                }
            }
        }

        public TaskModel SelectedTask 
        {
            get => _selectedTask;
            set 
            {
                _selectedTask = value;
                RaisePropertyChanged("ID");
                RaisePropertyChanged("CreationTime");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("DueDate");
                RaisePropertyChanged("EmailAssignee");
            } 
        }

        public IList<TaskModel> Tasks { get => _tasks; }
        public InProgressTasksViewModel(IList<TaskModel> tasks, BackendController controller) : base(controller)
        {
            this._tasks = tasks;
        }
    }
}
