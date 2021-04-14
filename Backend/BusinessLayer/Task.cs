    using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Task 
    {
        //fileds
        private readonly int taskId;
        internal int TaskId { get { return taskId; } }

        private readonly DateTime creationTime;
        internal DateTime CreationTime { get { return creationTime; } }

        private DateTime dueDate;
        internal DateTime DueDate { get { return dueDate; } set {
                ValidateDueDate(value);
                dueDate = value; } }


        private string title;
        internal string Title
        {
            get { return title; }
            set
            {
                ValidateTitle(value);
                title = value;
            }
        }

        private string description;
        internal string Description
        {
            get { return description; }
            set
            {
                ValidateDescription(value);
                this.description = value;
            }
        }

        //constructor
        public Task(int taskId,DateTime creationTime,  string title, string description, DateTime dueDate)
        {
            this.taskId = taskId;
            this.creationTime = creationTime;
            DueDate = dueDate;
            Title = title;
            Description = description;
        }

        //functions

        ///<summary>Validate the propriety of a given description.</summary>
        ///<param name="description">The description given to the Task</param>
        ///<exception cref="Exception">thrown when description is longer then 500 characters.</exception>
        private void ValidateDescription(string description)
        {
            if(description == null)
            {
                throw new ArgumentNullException("description cant be null, if you dont need description please enter empty string");
            }
            if(description.Length > 500)
            {
                throw new FormatException("Description max length is 500 characters");
            }   
        }

        ///<summary>Validate the propriety of a given title.</summary>
        /// <param name="title">The title given to the Task</param>
        /// <exception cref="ArgumentNullException">Thrown when title is null object </exception> 
        /// <exception cref="FormatException"> Thrown when the title dont answer are format requairements (empty, longer then 50)</exception>
        private void ValidateTitle(string title)
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

        private void ValidateDueDate(DateTime dueDate)
        {
            if (dueDate == null)
            {
                throw new ArgumentNullException("Due date must not be null");
            }
            if (dueDate < DateTime.Now)
            {
                throw new ArgumentException("This due date time already past");
            }
            
        }
    }
}
