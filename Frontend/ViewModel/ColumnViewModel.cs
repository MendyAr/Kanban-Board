using IntroSE.Kanban.Frontend.Model;
namespace IntroSE.Kanban.Frontend.ViewModel
{
    class ColumnViewModel : NotifiableObject
    {

        private ColumnModel _column;
        private TaskModel _selectedTask;

        private bool _enableForward;
        private string _filter;

        public ColumnModel Column { get => _column; set => _column = value; }

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
        // constructor

        public ColumnViewModel(ColumnModel column)
        {
            this._column = column;
        }

        //  methods

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
