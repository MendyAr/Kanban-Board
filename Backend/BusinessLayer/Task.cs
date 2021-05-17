using IntroSE.Kanban.Backend.DataLayer;
using System;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class Task 
    {
        private const int MIN_TITLE_LENGTH =  1;
        private const int MAX_TITLE_LENGTH = 50;
        private const int MIN_DESCRIPTION_LENGTH = 0;
        private const int MAX_DESCRIPTION_LENGTH = 300;
        
        //fields
        private readonly int taskId;
        private string title;
        private string description;
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private string assignee;

        private DTask dTask; //parallel DTO

        internal int TaskId { get => taskId;}
        internal DateTime CreationTime { get => creationTime;}
        internal DateTime DueDate { get => dueDate;  
            set 
            {
                validateDueDate(value);
                dTask.DueDate = value;
                dueDate = value; 
            } 
        }
        internal string Title { get => title; 
            set
            {
                validateTitle(value);
                dTask.Title = value;
                title = value;

            }
        }
        internal string Description { get => description; 
            set
            {
                validateDescription(value);
                dTask.Description = value;
                description = value;
            }
        }
        public string Assignee { get => assignee; 
            set 
            {
                dTask.Assignee = value;
                assignee = value;
            } 
        }


        //constructors
        /// <summary>
        /// Creates a new Task
        /// </summary>
        /// <param name="taskId">ID of Task</param>
        /// <param name="creationTime">Creation Time of task</param>
        /// <param name="title">Title of task - has to stand conditions</param>
        /// <param name="description">Description of task - has to stand conditions</param>
        /// <param name="dueDate">Due date of task - has to stand conditions</param>
        /// <param name="assignee">Assignee of task</param>
        /// <param name="boardCreator">Creator of the board the task is in - delivered to the created DTO</param>
        /// <param name="boardName">Board name of the board the task is in - delivered to the created DTO</param>
        /// <remarks>calls ValidateDueDate, ValidateTitle, ValidateDescription</remarks>
        internal Task(int taskId, DateTime creationTime,  string title, string description, DateTime dueDate, string assignee, string boardCreator, string boardName)
        {
            this.taskId = taskId;
            this.creationTime = creationTime;
            validateDueDate(dueDate);
            this.dueDate = dueDate;
            validateTitle(title);
            this.title= title;
            validateDescription(description);
            this.description = description;
            this.assignee = assignee;
            dTask = new DTask(taskId, creationTime, title, description, dueDate, assignee, 0, boardCreator, boardName);
            dTask.Insert();
            dTask.Persist = true;
        }

        /// <summary>
        /// Recreates Task from DTO
        /// </summary>
        /// <param name="dTask">DTO representing the task</param>
        internal Task(DTask dTask)
        {
            taskId = dTask.TaskId;
            creationTime = dTask.CreationTime;
            dueDate = dTask.DueDate;
            title = dTask.Title;
            description = dTask.Description;
            assignee = dTask.Assignee;
            this.dTask = dTask;
            this.dTask.Persist = true;
        }

        //methods

        /// <summary>
        /// Sends a message to dTask to advance column-wise
        /// </summary>
        internal void Advance()
        {
            dTask.Ordinal = dTask.Ordinal + 1;
        }

        ///<summary>Validates the property of a given description.</summary>
        ///<param name="description">The description given to the Task</param>
        ///<exception cref="ArgumentNullException">Thrown if given description is null</exception>
        ///<exception cref="FormatException">Thrown if the description doesn't fit limits</exception>
        private void validateDescription(string description)
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

        ///<summary>Validates the property of a given title.</summary>
        ///<param name="title">The title given to the Task</param>
        ///<exception cref="ArgumentNullException">Thrown if given title is null</exception>
        ///<exception cref="FormatException">Thrown if the title doesn't fit limits</exception>
        private void validateTitle(string title)
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
        /// <param name="dueDate">The dueDate given to the Task</param>
        /// <exception cref="ArgumentNullException">Thrown when dueDate is null object </exception> 
        /// <exception cref="FormatException"> Thrown when the dueDate is earlier then now. </exception>
        private void validateDueDate(DateTime dueDate)
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
