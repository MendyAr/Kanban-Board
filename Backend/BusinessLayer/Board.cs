using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    /// <summary>
    /// Contains 3 columns: Backlog, In Progress, Done
    /// manages those 3 columns and the addition/movement of tasks in them
    /// </summary>
    /// <remarks>in methods requesting columnOrdinal - the integer will represent 1 out of the 3 columns: 0 - Backlog, 1 - In Progress, 2 - Done</remarksY>
    internal class Board
    {
        //fields
        private int taskIdCounter = 1; //will be updated by every task added to the board and so keeping each ID unique
        private Column[] columns;

        //constructor
        /// <summary>
        /// creates a new Board object
        /// </summary>
        internal Board()
        {
            this.columns = new Column[] { new Column("backlog"), new Column("in progress"), new Column("done")};
        }

        //methods

        /// <summary>
        /// returns the name of a selected column
        /// </summary>
        /// <param name="columnOrdinal">integer representing column</param>
        /// <returns>name of a column</returns>
        internal string GetColumnName(int columnOrdinal)
        {
            return columns[columnOrdinal].ColumnName;
        }

        /// <summary>
        /// limits a column within the board
        /// </summary>
        /// <param name="columnOrdinal">represent the limited column</param>
        /// <param name="limit">new limit</param>
        internal void LimitColumn(int columnOrdinal, int limit)
        {
            columns[columnOrdinal].Limit = limit;
        }

        /// <summary>
        /// Gets the limit of a column within the board
        /// </summary>
        /// <param name="columnOrdinal">represent the limited column</param>
        /// <returns>the column's limit</returns>
        internal int GetColumnLimit(int columnOrdinal)
        {
            return columns[columnOrdinal].Limit;
        }
        
        /// <summary>
        /// Adds a new task to the board
        /// </summary>
        /// <remarks>the added task will always be added to the Backlog column</remarks>
        /// <param name="creationTime">task's creation time</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="DueDate">task's due date</param>
        /// <returns>the newly added task</returns>
        internal Task AddTask(DateTime creationTime, string title, string description, DateTime DueDate)
        {
            Task task = columns[0].AddTask(taskIdCounter, creationTime, title, description, DueDate);
            taskIdCounter++;
            return task;
        }
        
        /// <summary>
        /// updates an existing task's due date
        /// </summary>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="DueDate">new and updated due date</param>
        internal void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime DueDate)
        {
            columns[columnOrdinal].UpdateTaskDueDate(taskId, DueDate);
        }

        /// <summary>
        /// updates an existing task's title
        /// </summary>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new and updated title</param>
        internal void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
        {
            columns[columnOrdinal].UpdateTaskTitle(taskId, title);
        }
        
        /// <summary>
        /// updates an existing task's description
        /// </summary>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new and updated description</param>
        internal void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
        {
            columns[columnOrdinal].UpdateTaskDescription(taskId, description);
        }
        
        /// <summary>
        /// advances a task to the next column
        /// </summary>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">the advanced task's ID</param>
        /// <exception cref="OutOfMemoryException">Thrown if the next column is already at its limit</exception>
        internal void AdvanceTask(int columnOrdinal, int taskId)
        {
            Task task = columns[columnOrdinal].RemoveTask(taskId); //removes task from current column
            try
            {
                columns[columnOrdinal + 1].AddTask(task); //adds the task to the next column
            }
            catch (OutOfMemoryException e) //if the next column is already at its limit
            {
                columns[columnOrdinal].AddTask(task); //returns the task to its original column
                throw new OutOfMemoryException(e.Message); //sends forward the same exception
            }
        }
        
        /// <summary>
        /// returns all the tasks in a requested column
        /// </summary>
        /// <param name="columnOrdinal">represents the requested column</param>
        /// <returns>IList containing all the tasks in the column</returns>
        internal IList<Task> GetColumn(int columnOrdinal)
        {
            return columns[columnOrdinal].GetColumn();
        }
    }
}
