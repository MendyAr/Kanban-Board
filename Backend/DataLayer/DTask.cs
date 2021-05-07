using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTask : DTO
    {
        private static string _titleColumnName = "Title";
        private static string _descriptionColumnName = "Description";
        private static string _dueDateColumnName = "DueDate";


        private int taskId;
        private string boardName;
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        public DTask(int taskId, string boardName, string title, string description, DateTime creationTime, DateTime dueDate) : base(new DTaskController())
        {
            this.boardName = boardName;
            this.taskId = taskId;
            Title = title;
            Description= description;
            this.creationTime = creationTime;
            DueDate= dueDate;
        }
        public int TaskId
        { get { return taskId; } }


        public string Title
        {
            get { return title; }
            set
            {
                if (persist)
                {
                    _controller.Update(TaskId, boardName, _titleColumnName, value);
                }
                title = value;
            }
        }

        public string Description
        {
            get { return description; }
            set
            {

                if (persist)
                {
                    _controller.Update(TaskId, boardName, _descriptionColumnName, value);
                }
                description = value;
            }
        }

        public DateTime CreationTime { get { return creationTime; } }

        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {

                if (persist)
                {
                    _controller.Update(TaskId, boardName, _dueDateColumnName, value);
                }
                dueDate = value;
            }
        }

        
    }
}
