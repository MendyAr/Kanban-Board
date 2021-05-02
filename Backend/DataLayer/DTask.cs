using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTask : DTO
    {
        private int taskId;
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        public DTask(int taskId,string title,string description, DateTime creationTime, DateTime dueDate, bool persisted) : base(new DTaskController(),persisted)
        {
            this.taskId = taskId;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
        }
    }
}
