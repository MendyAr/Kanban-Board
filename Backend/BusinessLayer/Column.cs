using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    /// <summary>
    /// Class Column stores tasks and allows column operations and manipulation.
    /// </summary>
    /// <remarks>All exceptions thrown will message the column's name for parsing uses of the calling method</remarks>
    internal class Column
    {
        //fields
        private string columnName;
        private int limit; //ranges from -1 (unlimited) to and positive number (actual limit)
        private Dictionary<int, Task> column; //key - task's ID

        internal string ColumnName { get => columnName;  }
        internal int Limit
        {
            get => limit;
            set
            {
                 if (value != -1 && column.Count > value) 
                     throw new ArgumentException(columnName);
                 this.limit = value;
            }
        }

        //constructors
        internal Column(string columnName)
        {
            this.columnName = columnName;
            this.limit = -1; 
            this.column = new Dictionary<int, Task>();
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
        internal Task AddTask(int taskId, DateTime creationTime, string title, string description, DateTime DueDate) {
            //no need to check that limit != -1 as then the boolean check will always be false anyway
            if (column.Count == limit)
                throw new OutOfMemoryException(columnName);
            column[taskId] = new Task(taskId, creationTime, title, description, DueDate);
            return column[taskId];
        }

        /// <summary>
        /// Adds the given task to the column
        /// </summary>
        /// <param name="task">the added task</param>
        /// <exception cref="OutOfMemoryException">Thrown when column is already at its limit</exception>
        internal void AddTask(Task task)
        {
            //no need to check that limit != -1 as then the boolean check will always be false anyway
            if (column.Count == limit)
                throw new OutOfMemoryException(columnName);
            column[task.TaskId] = task;
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
                removedTask = column[taskId];
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(columnName);
            }
            column.Remove(taskId);
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
                column[taskId].DueDate = DueDate;
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(columnName);
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
                column[taskId].Title = title;
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(columnName);
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
                column[taskId].Description = description;
            }
            catch (KeyNotFoundException)
            {
                throw new IndexOutOfRangeException(columnName);
            }
        }

        /// <summary>
        /// Returns all the tasks contained in the column
        /// </summary>
        /// <returns>IList of all the tasks in the column</returns>
        internal IList<Task> GetColumn()
        {
            IList<Task> column = new List<Task>();
            foreach (Task task in this.column.Values)
            {
                column.Add(task);
            }
            return column;
        }
    }
}
