using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    class TaskViewModel : ViewModelObject
    {
        public TaskViewModel(TaskModel task) : base(task.Controller)
        {
        }
    }
}
