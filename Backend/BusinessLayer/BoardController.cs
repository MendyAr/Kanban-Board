using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class BoardController
    {
        //fields
        private Dictionary<string, Dictionary<string, Board>> boards ;

        //constructors
        public BoardController()
        {
            boards = new Dictionary<string, Dictionary<string, Board>>();
        }

        //methods
        public void Register(string email)
        {
            boards[email] = new Dictionary<string, Board>();
        }

        public void AddBoard(string email, string boardName)
        {
            if (boards[email].ContainsKey(boardName))
                throw new ArgumentException("Board '" + email + ":" + boardName + "' already exist");
            boards[email][boardName] = new Board();
        }

        public void RemoveBoard(string email, string boardName)
        {
            if (!boards[email].ContainsKey(boardName))
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            boards[email].Remove(boardName);
        }

        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            checkColumnOrdinal(columnOrdinal);
            if (limit < -1)
                throw new ArgumentException("impossible limit");
            try
            {
                boards[email][boardName].LimitColumn(columnOrdinal, limit);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("Cannot set limit: There are more than " + limit + " tasks in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
        }

        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            checkColumnOrdinal(columnOrdinal);
            try
            {
                return boards[email][boardName].GetColumnLimit(columnOrdinal);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
        }

        public Task AddTask(string email, string boardName, DateTime creationTime, string title, string description, DateTime dueDate)
        {
            try
            {
                return boards[email][boardName].AddTask(creationTime, title, description, dueDate);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
            catch (OutOfMemoryException e)
            {
                throw new OutOfMemoryException("Cannot add task: Column '" + e.Message + "' of board '" + email + ":" + boardName + "' is currently at its limit");
            }
        }

        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            checkColumnOrdinal(columnOrdinal);
            try
            {
                boards[email][boardName].UpdateTaskDueDate(columnOrdinal, taskId, dueDate);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot update task: A task with ID '" + taskId +"' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            checkColumnOrdinal(columnOrdinal);
            try
            {
                boards[email][boardName].UpdateTaskTitle(columnOrdinal, taskId, title);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot update task: A task with ID '" + taskId +"' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            checkColumnOrdinal(columnOrdinal);
            try
            {
                boards[email][boardName].UpdateTaskDescription(columnOrdinal, taskId, description);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot update task: A task with ID '" + taskId +"' does not exist in column '" + e.Message + "' of board '" + email + ":" + boardName + "'");
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            checkColumnOrdinal(columnOrdinal);
            if (columnOrdinal == 2)
                throw new ArgumentException("Cannot advance tasks from column 'Done'");
            try
            {
                boards[email][boardName].AdvanceTask(columnOrdinal, taskId);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
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
        
        public IList<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            checkColumnOrdinal(columnOrdinal);
            try
            {
                return boards[email][boardName].GetColumn(columnOrdinal);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Board '" + email + ":" + boardName+ "' does not exist");
            }
        }
        
        public IList<Task> InProgressTasks(string email)
        {
            IList<Task> inProgress = new List<Task>();
            foreach (Board board in boards[email].Values)
            {
                concatLists(inProgress, board.GetColumn(1));
            }
            return inProgress;
        }

        private void concatLists(IList<Task> addTo, IList<Task> addFrom)
        {
            foreach (Task task in addFrom)
                addTo.Add(task);
        }

        private void checkColumnOrdinal(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
                throw new IndexOutOfRangeException("Column ordinal out of range: Argument needs to be between 0 and 2 (inclusive)");
        }
    }
}