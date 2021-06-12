using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class TaskViewModel : ViewModelObject
    {
        private string _userEmail;
        private string _boardCreator;
        private string _boardName;
        private int _columnOrdinal;
        private int _taskID;
        private string _title;
        private string _assignee;
        private string _description;
        private string _creationDate;
        private DateTime _dueDate;

        public string Title
        {
            get => _title;
            set
            {
                try
                {
                    UpdateTitle()
                }
            }
        }
    }
}
