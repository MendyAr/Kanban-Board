using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Task 
    {
        private readonly int taskId;
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;

       public Task(int taskId, DateTime dueDate, string title, string description)
        {
            this.taskId = taskId;
            this.creationTime = DateTime.Now;
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;

            // log.add("create new Task)
        }
        internal int TaskId { get { return taskId; } }

        internal DateTime CreationTime { get { return creationTime; }}
        internal DateTime DueDate { get { return dueDate; } set { dueDate = value; } }

        internal string Title { get { return title; } set { title = value; } }

        internal string Description { get { return description; } set { description = value; } }
    }
}
