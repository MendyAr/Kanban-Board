    using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Task 
    {
        //fileds
        private static int MIN_TITLE_LENGTH = 1;
        private static int MAX_TITLE_LENGTH = 50;
        private static int MIN_DESCRIPTION_LENGTH = 0;
        private static int MAX_DESCRIPTION_LENGTH = 300;

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
            if (description == null)
            {
                throw new ArgumentNullException("Description must not be null");
            }
            if(description.Length > MAX_DESCRIPTION_LENGTH)
            {
                throw new FormatException("Description max length is" + MAX_DESCRIPTION_LENGTH + "characters");
            }

            if (description.Length < MIN_DESCRIPTION_LENGTH)
            {
                throw new FormatException("Description minimum length is" + MIN_DESCRIPTION_LENGTH + "characters");
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
            if (title.Length < MIN_TITLE_LENGTH) 
            {
                throw new FormatException("Title must contain at list " + MIN_TITLE_LENGTH + " character");
            }
            if (title.Length > MAX_TITLE_LENGTH)
            {
                throw new FormatException("Title length cannot be more then " + MAX_TITLE_LENGTH + " characters");
            }
        }

        ///<summary>Validate the propriety of a given dueDate.</summary>
        /// <param name="dueDate">The title given to the Task</param>
        /// <exception cref="ArgumentNullException">Thrown when dueDate is null object </exception> 
        /// <exception cref="FormatException"> Thrown when the dueDate is earlier then now. </exception>
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
