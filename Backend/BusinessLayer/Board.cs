using System;
using System.Collections.Generic;
using DBoard = IntroSE.Kanban.Backend.DataLayer.DBoard;
using DColumn = IntroSE.Kanban.Backend.DataLayer.DColumn;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// Contains 3 columns: Backlog, In Progress, Done
    /// manages those 3 columns and the addition/movement of tasks in them
    /// </summary>
    /// <remarks>in methods requesting columnOrdinal - the integer will represent 1 out of the 3 columns: 0 - Backlog, 1 - In Progress, 2 - Done</remarksY>
    internal class Board
    {
        //fields
        private int taskIdCounter = 0; //will be updated by every task added to the board and so keeping each ID unique
        private IList<Column> columns = new List<Column>();

        private DBoard dBoard; //parallel DTO

        internal int TaskIdCounter { get => taskIdCounter; }
        internal IList<string> Columns
        {
            get
            {
                IList<string> colNames = new List<string>();
                foreach (Column column in columns)
                {
                    colNames.Add(column.Name);
                }
                return colNames;
            }
        }

        //constructor
        /// <summary>
        /// creates a new Board object
        /// </summary>
        internal Board(string creatorEmail, string boardName)
        {
            columns.Insert(0, new Column("backlog", creatorEmail, boardName, 0));
            columns.Insert(1, new Column("in progress", creatorEmail, boardName, 1)); 
            columns.Insert(2, new Column("done", creatorEmail, boardName, 2));
            dBoard = new DBoard(creatorEmail, boardName);
            dBoard.Insert();
            dBoard.Persist = true;
        }

        internal Board(DBoard dBoard)
        {
            columns.Insert(0, new Column(dBoard.Columns[0]));
            columns.Insert(1, new Column(dBoard.Columns[1])); 
            columns.Insert(2, new Column(dBoard.Columns[2]));
            taskIdCounter = dBoard.numberOfTasks();
            this.dBoard = dBoard;
            this.dBoard.Persist = true;
        }

        //methods

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
        /// Adds a new task to the board at column 'Backlog'
        /// </summary>
        /// <param name="creationTime">Creation Time of task</param>
        /// <param name="title">Title of task - has to stand conditions</param>
        /// <param name="description">Description of task - has to stand conditions</param>
        /// <param name="dueDate">Due date of task - has to stand conditions</param>
        /// <param name="assignee">Assignee of task</param>
        /// <param name="boardCreator">Creator of the board the task is in - delivered to the created DTO</param>
        /// <param name="boardName">Board name of the board the task is in - delivered to the created DTO</param>
        /// <remarks>the added task will always be added to the Backlog column</remarks>
        /// <returns>the newly added task</returns>
        internal Task AddTask(DateTime creationTime,  string title, string description, DateTime dueDate, string assignee, string boardCreator, string boardName) 
        {
            Task task = columns[0].AddTask(taskIdCounter, creationTime, title, description, dueDate, assignee, boardCreator, boardName);
            taskIdCounter++;
            return task;
        }
        
        /// <summary>
        /// updates an existing task's assignee
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="assignee">new assignee</param>
        internal void AssignTask(string userEmail, int columnOrdinal, int taskId, string assignee)
        {
            columns[columnOrdinal].AssignTask(userEmail, taskId, assignee);
        }
        
        /// <summary>
        /// updates an existing task's due date
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="DueDate">new and updated due date</param>
        internal void UpdateTaskDueDate(string userEmail, int columnOrdinal, int taskId, DateTime DueDate)
        {
            columns[columnOrdinal].UpdateTaskDueDate(userEmail, taskId, DueDate);
        }

        /// <summary>
        /// updates an existing task's title
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new and updated title</param>
        internal void UpdateTaskTitle(string userEmail, int columnOrdinal, int taskId, string title)
        {
            columns[columnOrdinal].UpdateTaskTitle(userEmail, taskId, title);
        }
        
        /// <summary>
        /// updates an existing task's description
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new and updated description</param>
        internal void UpdateTaskDescription(string userEmail, int columnOrdinal, int taskId, string description)
        {
            columns[columnOrdinal].UpdateTaskDescription(userEmail, taskId, description);
        }
        
        /// <summary>
        /// advances a task to the next column
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="columnOrdinal">which column the task is in</param>
        /// <param name="taskId">the advanced task's ID</param>
        /// <exception cref="OutOfMemoryException">Thrown if the next column is already at its limit</exception>
        internal void AdvanceTask(string userEmail, int columnOrdinal, int taskId)
        {
            Task task = columns[columnOrdinal].RemoveTask(userEmail, taskId); //removes task from current column
            try
            {
                columns[columnOrdinal + 1].AddTask(task); //adds the task to the next column
                task.Advance();
            }
            catch (OutOfMemoryException e) //if the next column is already at its limit
            {
                columns[columnOrdinal].AddTask(task); //returns the task to its original column
                throw new OutOfMemoryException(e.Message); //sends forward the same exception
            }
        }
        
        /// <summary>
        /// Returns specific column
        /// </summary>
        /// <param name="columnOrdinal">represents the requested column</param>
        /// <returns>Requested Column</returns>
        internal Column GetColumn(int columnOrdinal)
        {
            return columns[columnOrdinal];
        }

        /// <summary>
        /// Returns all the tasks of spcified column
        /// </summary>
        /// <param name="columnOrdinal">represents the requested column</param>
        /// <returns>IList<Task> containing all the tasks</Task></returns>
        internal IList<Task> GetColumnTasks(int columnOrdinal)
        {
            return columns[columnOrdinal].Tasks;
        }
    }
}
