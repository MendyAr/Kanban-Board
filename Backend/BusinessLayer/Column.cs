using System;
using System.Collections.Generic;

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
        private int limit = -1; //ranges from -1 (unlimited) to and positive number (actual limit)

        internal string Name { get => name; set => name = value; }
        internal int Limit
        {
            get => limit;
            set
            {
                 if (value != -1 && tasks.Count > value) 
                     throw new ArgumentException(Name);
                 limit = value;
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
        internal Column(string Name)
        {
            this.Name = Name;
        }

        //methods

        /// <summary>
        /// Adds a new task that represents given parameters to the column
        /// </summary>
        /// <param name="taskId">ID of the task</param>
        /// <param name="creationTime">Task's creation time</param>
        /// <param name="title">Task's title</param>
        /// <param name="description">Task's description</param>
        /// <param name="DueDate">Task's due date</param>
        /// <returns>The new task that was created</returns>
        /// <exception cref="OutOfMemoryException">Thrown when column is already at its limit</exception>
        internal Task AddTask(int taskId, string Assignee, DateTime creationTime, string title, string description, DateTime DueDate) {
            //no need to check that limit != -1 as then the boolean check will always be false anyway
            if (tasks.Count == limit)
                throw new OutOfMemoryException(Name);
            tasks[taskId] = new Task(taskId, Assignee, creationTime, title, description, DueDate);
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
        /// Removes task from the column
        /// </summary>
        /// <param name="taskId">ID of the removed task</param>
        /// <returns>the removed task</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the column doesn't hold a task with matching given ID</exception>
        internal Task RemoveTask(int taskId)
        {
            Task removedTask = null;
            try
            {
                removedTask = tasks[taskId];
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(Name);
            }
            tasks.Remove(taskId);
            return removedTask;
        }

        /// <summary>
        /// Updates a contained task's due date
        /// </summary>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="DueDate">new and updated due date</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the column doesn't hold a task with matching given ID</exception>
        internal void UpdateTaskDueDate(int taskId, DateTime DueDate)
        {
            try
            {
                tasks[taskId].DueDate = DueDate;
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(Name);
            }
        }

        /// <summary>
        /// Updates a contained task's title
        /// </summary>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="title">new and updated title</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the column doesn't hold a task with matching given ID</exception>
        internal void UpdateTaskTitle(int taskId, string title)
        {
            try
            {
                tasks[taskId].Title = title;
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(Name);
            }
        }

        /// <summary>
        /// Updates a contained task's description
        /// </summary>
        /// <param name="taskId">ID of updated task</param>
        /// <param name="title">new and updated description</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the column doesn't hold a task with matching given ID</exception>
        internal void UpdateTaskDescription(int taskId,  string description)
        {
            try
            {
                tasks[taskId].Description = description;
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(Name);
            }
        }
    }
}
