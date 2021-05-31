using System;
using System.Collections.Generic;
using DColumn = IntroSE.Kanban.Backend.DataLayer.DColumn;
using DTask = IntroSE.Kanban.Backend.DataLayer.DTask;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// Class Column stores tasks and allows column operations and manipulation.
    /// </summary>
    /// <remarks>All exceptions thrown will message the column's name for parsing uses of the calling method</remarks>
    internal class Column
    {
        //fields
        private string name;
        private Dictionary<int, Task> tasks = new Dictionary<int, Task>(); //key - task's ID
        private int limit = -1; //ranges from -1 (unlimited) to any positive number (actual limit)

        private DColumn dColumn; //parallel DTO

        internal string Name { 
            get => name;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Column Name cannot be null");
                if (value.Equals(""))
                    throw new ArgumentException("New Name cannot be empty");
                name = value;
                dColumn.Name = value;

            }
        }
        internal int Limit
        {
            get => limit;
            set
            {
                if (value != -1 && tasks.Count > value) 
                    throw new ArgumentException(Name);
                limit = value;
                dColumn.Limit = value;
            }
        }
        internal IList<Task> Tasks
        {
            get
            {
                IList<Task> tasks = new List<Task>();
                foreach (Task task in this.tasks.Values)
                {
                    tasks.Add(task);
                }
                return tasks;
            }
        }

        //constructors
        internal Column(string name, string creatorEmail, string boardName, int columnOrdinal)
        {
            this.name = name;
            dColumn = new DColumn(name, creatorEmail, boardName, columnOrdinal, -1);
            dColumn.Insert();
            dColumn.Persist = true;
        }

        /// <summary>
        /// Recreates Column from DTO
        /// </summary>
        /// <param name="dColumn">DTO representing the column</param>
        internal Column(DColumn dColumn)
        {
            limit = dColumn.Limit;
            name = dColumn.Name;
            foreach (DTask dTask in dColumn.Tasks)
            {
                tasks[dTask.TaskId] = new Task(dTask);
            }
            this.dColumn = dColumn;
            this.dColumn.Persist = true;
        }

        //methods

        /// <summary>
        /// Adds a new task that represents given parameters to the column
        /// </summary>
        /// <param name="taskId">ID of Task</param>
        /// <param name="creationTime">Creation Time of task</param>
        /// <param name="title">Title of task - has to stand conditions</param>
        /// <param name="description">Description of task - has to stand conditions</param>
        /// <param name="dueDate">Due date of task - has to stand conditions</param>
        /// <param name="assignee">Assignee of task</param>
        /// <param name="boardCreator">Creator of the board the task is in - delivered to the created DTO</param>
        /// <param name="boardName">Board name of the board the task is in - delivered to the created DTO</param>
        /// <returns>The new task that was created</returns>
        /// <exception cref="OutOfMemoryException">Thrown when column is already at its limit</exception>
        internal Task AddTask(int taskId, DateTime creationTime,  string title, string description, DateTime dueDate, string assignee, string boardCreator, string boardName) { 
            //no need to check that limit != -1 as then the boolean check will always be false anyway
            if (tasks.Count == limit)
                throw new OutOfMemoryException(Name);
            tasks[taskId] = new Task(taskId, creationTime, title, description, dueDate, assignee, boardCreator, boardName);
            return tasks[taskId];
        }

        /// <summary>
        /// Adds the given task to the column
        /// </summary>
        /// <param name="task">the added task</param>
        /// <exception cref="OutOfMemoryException">Thrown when column is already at its limit</exception>
        internal void AddTask(Task task)
        {
            //no need to check that limit != -1 as then the boolean check will always be false anyway
            if (tasks.Count == limit)
                throw new OutOfMemoryException(Name);
            tasks[task.TaskId] = task;
        }

        /// <summary>
        /// adds all given tasks in the IList to the column
        /// </summary>
        /// <param name="tasks">new tasks</param>
        internal void AddTasks(IList<Task> tasks)
        {
            if (limit != -1 && (tasks.Count + this.tasks.Count) > limit)
                throw new OutOfMemoryException(Name);
            foreach (Task task in tasks)
            {
                tasks[task.TaskId] = task;
            }
        }

        /// <summary>
        /// Removes task from the column
        /// </summary>
        /// <param name="taskId">ID of the removed task</param>
        /// <returns>the removed task</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the column doesn't hold a task with matching given ID</exception>
        internal Task RemoveTask(string userEmail, int taskId)
        {
            validateAssignee(userEmail, taskId);
            Task removedTask = tasks[taskId];
            tasks.Remove(taskId);
            return removedTask;
        }

        /// <summary>
        /// Updates a contained task's due date
        /// </summary>
        /// <param name="userEmail">callig user's email</param>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="assignee">new assignee</param>
        /// <remarks>calls validateAssignee</remarks>
        internal void AssignTask(string userEmail, int taskId, string assignee)
        {
            validateAssignee(userEmail, taskId);
            tasks[taskId].Assignee = assignee;
        }

        /// <summary>
        /// Updates a contained task's due date
        /// </summary>
        /// <param name="userEmail">callig user's email</param>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="DueDate">new and updated due date</param>
        /// <remarks>calls validateAssignee</remarks>
        internal void UpdateTaskDueDate(string userEmail, int taskId, DateTime DueDate)
        {
            validateAssignee(userEmail, taskId);
            tasks[taskId].DueDate = DueDate;
        }

        /// <summary>
        /// Updates a contained task's title
        /// </summary>
        /// <param name="userEmail">callig user's email</param>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="title">new and updated title</param>
        /// <remarks>calls validateAssignee</remarks>
        internal void UpdateTaskTitle(string userEmail, int taskId, string title)
        {
            validateAssignee(userEmail, taskId);
            tasks[taskId].Title = title;
        }

        /// <summary>
        /// Updates a contained task's description
        /// </summary>
        /// <param name="userEmail">callig user's email</param>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="description">new and updated description</param>
        /// <remarks>calls validateAssignee</remarks>
        internal void UpdateTaskDescription(string userEmail, int taskId,  string description)
        {
            validateAssignee(userEmail, taskId);
            tasks[taskId].Description = description;
        }

        /// <summary>
        /// updates the ordinal saved in DAL
        /// </summary>
        /// <param name="columnOrdinal">new ordinal</param>
        internal void UpdateOrdinal(int columnOrdinal)
        {
            dColumn.Ordinal = columnOrdinal;
        }

        /// <summary>
        /// Validates the working user is the assignee of the task
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="taskId">task's ID</param>
        private void validateAssignee(string userEmail, int taskId)
        {
            try
            {
                if (!tasks[taskId].Assignee.Equals(userEmail))
                    throw new InvalidOperationException();
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(Name);
            }
        }
    }
}
