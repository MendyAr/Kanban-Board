using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    /// <summary>
    /// A collection of all the boards in the system. Manages operationg within specific boards and general board related checks
    /// </summary>
    /// <remarks>in methods requesting columnOrdinal - the integer will represent 1 out of the 3 columns: 0 - Backlog, 1 - In Progress, 2 - Done</remarksY>
    internal class BoardController
    {
        //fields
        private Dictionary<string, Dictionary<string, Board>> boards ; //first key will be the email of each user, second key will be the board name

        //constructors
        internal BoardController()
        {
            boards = new Dictionary<string, Dictionary<string, Board>>();
        }

        //methods

        /// <summary>
        /// creates a new entry for a newly registered user
        /// </summary>
        /// <param name="email">the newly registered user's email</param>
        internal void Register(string email)
        {
            boards[email] = new Dictionary<string, Board>();
        }

        /// <summary>
        /// adds a new board for a user
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">the new board name</param>
        /// <exception cref="ArgumentException">thrown when the board already exists for that user</exception>
        internal void AddBoard(string email, string boardName)
        {
            if (boards[email].ContainsKey(boardName))
                throw new ArgumentException("Board '" + email + ":" + boardName + "' already exist");
            boards[email][boardName] = new Board();
        }

        /// <summary>
        /// deletes an existing board
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">the deleted board's name</param>
        /// <exception cref="ArgumentException">thrown when trying to delete a non-existing board</exception>
        internal void RemoveBoard(string email, string boardName)
        {
            if (!boards[email].ContainsKey(boardName))
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            boards[email].Remove(boardName);
        }

        /// <summary>
        /// gets the name of a specified column
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the column is stored</param>
        /// <param name="columnOrdinal">the column index</param>
        /// <returns>the name of the column</returns>
        /// <exception cref="ArgumentException">throw if the board does now exist</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            try
            {
                return boards[email][boardName].GetColumnName(columnOrdinal);
            }
        }

        /// <summary>
        /// sets a limit to a specific column in a specific board
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">the board in which the column is stored</param>
        /// <param name="columnOrdinal">the column</param>
        /// <param name="limit">new and updated limit</param>
        /// <exception cref="ArgumentException">thrown if the new limit isn't legal, if it's impossible to set the limit due to column complications</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            if (limit < -1)
                throw new ArgumentException("impossible limit");
            try
            {
                boards[email][boardName].LimitColumn(columnOrdinal, limit);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("Cannot set limit: There are more than " + limit + " tasks in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
        }

        /// <summary>
        /// gets the limit set to a specified column
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">the board in which the column is stored</param>
        /// <param name="columnOrdinal">index of the column</param>
        /// <returns>the column limit</returns>
        /// <exception cref="ArgumentException">throw if the board does now exist</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            return boards[email][boardName].GetColumnLimit(columnOrdinal);
        }

        /// <summary>
        /// adds a new task to a board (always adds the task to 'Backlog' column)
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">board to which the task need to be added</param>
        /// <param name="creationTime">task's creation time</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="DueDate">task's due date</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">throw if one of the task's arguments isn't legal</exception>
        /// <exception cref="OutOfMemoryException">thrown if the column is already at its limit</exception>
        /// <remarks>calls checkBoardExistance</remarks>
        internal Task AddTask(string email, string boardName, DateTime creationTime, string title, string description, DateTime DueDate)
        {
            checkBoardExistance(email, boardName);
            try
            {
                return boards[email][boardName].AddTask(creationTime, title, description, DueDate);
            }
            catch (OutOfMemoryException e)
            {
                throw new OutOfMemoryException("Cannot add task: Column '" + e.Message + "' of board '" + email + ":" + boardName + "' is currently at its limit");
            }
            catch (Exception e)
            {
                throw new ArgumentException("Cannot add task: " + e.Message);
            }
        }

        /// <summary>
        /// updates an existing task's due date
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="DueDate">new and updated due date</param>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new DueDate isn't legal</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime DueDate)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            try
            {
                boards[email][boardName].UpdateTaskDueDate(columnOrdinal, taskId, DueDate);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot update task: A task with ID '" + taskId +"' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (Exception e)
            {
                throw new ArgumentException("Cannot update task: " + e.Message);
            }
        }

        /// <summary>
        /// updates an existing task's title
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new and updated title</param>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new title isn't legal</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            try
            {
                boards[email][boardName].UpdateTaskTitle(columnOrdinal, taskId, title);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot update task: A task with ID '" + taskId +"' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (Exception e)
            {
                throw new ArgumentException("Cannot update task: " + e.Message);
            }
        }

        /// <summary>
        /// updates an existing task's description
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new and updated description</param>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new description isn't legal</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(boardName, email);
            try
            {
                boards[email][boardName].UpdateTaskDescription(columnOrdinal, taskId, description);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot update task: A task with ID '" + taskId + "' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// advanced an existing task's to the next column
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <exception cref="ArgumentException">Throw if asking to advance a task from 'Done' column, if the task isn't stored in said column, if next column is at it's limit</exception>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            if (columnOrdinal == 2)
                throw new ArgumentException("Cannot advance tasks from column 'Done'");
            try
            {
                boards[email][boardName].AdvanceTask(columnOrdinal, taskId);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot advance task: A task with ID '" + taskId +"' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (OutOfMemoryException e)
            {
                throw new OutOfMemoryException("Cannot advance task: Column '" + e.Message + "' of board '" + email + ":" + boardName + "' is currently at its limit");
            }
        }
        
        /// <summary>
        /// gets all the tasks in a specified column
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">the board in which the column is stored</param>
        /// <param name="columnOrdinal">column index</param>
        /// <returns>IList with all the tasks in the column</returns>
        /// <remarks>calls checkColumnOrdinal, checkBoardExistance</remarks>
        internal IList<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            checkColumnOrdinal(columnOrdinal);
            checkBoardExistance(email, boardName);
            return boards[email][boardName].GetColumn(columnOrdinal);
        }
        
        /// <summary>
        /// gets all the tasks of a user that are 'In Progress'
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <returns>IList of the user's 'In Progress' tasks</returns>
        /// <remarks>calls concatLists</remarks>
        internal IList<Task> InProgressTasks(string email)
        {
            IList<Task> inProgress = new List<Task>();
            foreach (Board board in boards[email].Values)
            {
                concatLists(inProgress, board.GetColumn(1));
            }
            return inProgress;
        }

        /// <summary>
        /// adds the content of one list to the other
        /// </summary>
        /// <param name="addTo">the list that the items will be added to</param>
        /// <param name="addFrom">the list that the items will be added from</param>
        private void concatLists(IList<Task> addTo, IList<Task> addFrom)
        {
            foreach (Task task in addFrom)
                addTo.Add(task);
        }

        /// <summary>
        /// checks legality of columnOrdinal
        /// </summary>
        /// <param name="columnOrdinal">columnOrdinal that needs to be checked</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the given ordinal isn't legal</exception>
        private void checkColumnOrdinal(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
                throw new IndexOutOfRangeException("Column ordinal out of range: Argument needs to be between 0 and 2 (inclusive)");
        }

        /// <summary>
        /// checks whether a board exists or not
        /// </summary>
        /// <param name="email">email of a user</param>
        /// <param name="boardName">board of the same user</param>
        /// <exception cref="ArgumentException">thrown if the board does not exist</exception>
        private void checkBoardExistance(string email, string boardName)
        {
            if (!boards[email].ContainsKey(boardName))
                throw new ArgumentException("Board '" + email + ":" + boardName + "' does not exist");
        }
    }
}