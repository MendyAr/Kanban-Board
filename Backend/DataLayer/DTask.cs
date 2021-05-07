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
        private static string _assignee = "Assignee";


        private readonly int taskId;
        public int TaskId
        { get { return taskId; } }


        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (persist)
                {
                    _controller.Update(TaskId, _titleColumnName, value);
                }
                title = value;
            }
        }


        private string description;
        public string Description
        {
            get { return description; }
            set
            {

                if (persist)
                {
                    _controller.Update(TaskId, _descriptionColumnName, value);
                }
                description = value;
            }
        }


        private readonly DateTime creationTime;
        public DateTime CreationTime { get { return creationTime; } }


        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {

                if (persist)
                {
                    _controller.Update(TaskId, _dueDateColumnName, value.ToString());
                }
                dueDate = value;
            }
        }


        private string assignee;
        public string Assignee { get { return assignee; } 
            set
            {
                if (persist)
                {
                    _controller.Update(taskId, _assignee, value);
                }
                assignee = value;
            } }


        public DTask(int taskId, string boardName, string title, string description, DateTime creationTime, DateTime dueDate, string assignee) : base(new DTaskController())
        {
            this.taskId = taskId;
            Title = title;
            Description= description;
            this.creationTime = creationTime;
            DueDate= dueDate;
            Assignee = assignee;
        }

        public override void insert()
        {
            _controller.insert(this);
        }
    }
}
