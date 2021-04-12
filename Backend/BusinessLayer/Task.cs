    using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Task 
    {
        //fileds
        private readonly int taskId;
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;

        //constructor
       public Task(int taskId,DateTime creationTime,  string title, string description, DateTime dueDate)
        {
            this.taskId = taskId;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            Title = title;
            Description = description;

            // log.add("create new Task)
        }

        //functions
        private void validateDescription(string description)
        {
            if(description != null && description.Length > 500)
            {
                throw new FormatException("Description max length is 500 characters");
            }   
        }

        internal int TaskId { get { return taskId; } }

        private void validateTitle(string title)
        {
            if(title == null)
            {
                throw new ArgumentNullException("Title must not be null");
            }
            if (title.Length == 0) 
            {
                throw new FormatException("Title cannot be empty");
            }
            if (title.Length > 50)
            {
                throw new FormatException("Title length cannot be more then 50 characters");
            }
        }

        internal DateTime CreationTime { get { return creationTime; }}
        internal DateTime DueDate { get { return dueDate; } set { dueDate = value; } }

        internal string Title { get { return title; } 
            set {
                validateTitle(value);
                title = value; } }

        internal string Description { get { return description; } 
            set {
                validateDescription(value);
                if(value == null)
                {
                    description = "";
                }
                else
                {
                    description = value;
                }
                 } }

    }
}
