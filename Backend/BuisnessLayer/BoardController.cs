using System;
using System.Collections.Generic;
using STask = IntroSE.Kanban.Backend.ServiceLayer.Task;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class BoardController
    {
        //fields
        private Dictionary<string, Dictionary<string, Board>> boards = new Dictionary<string, Dictionary<string, Board>>;

        //constructors
        public BoardController()
        {
        }

        //methods
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
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
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
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
            }

        }

        public STask AddTask(string email, string boardName, DateTime creationTime, string title, string description, DateTime dueDate)
        {
            try
            {
                return boards[email][boardName].AddTask(creationTime, title, description, dueDate);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException(e.Message);
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
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
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
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
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
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
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
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException(e.Message);
            }
        }
        
        public IList<STask> GetColumn(string email, string boardName, int columnOrdinal)
        {
            checkColumnOrdinal(columnOrdinal);
            try
            {
                return boards[email][boardName].GetColumn(columnOrdinal);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("There is no board with name '" + boardName + "' for user '" + email + "'");
            }
        }
        
        public void AddBoard(string email, string name)
        {
            if (boards[email].ContainsKey(name))
                throw new ArgumentException("A board with name '" + name + "' already exsist for user '" + email + "'");
            boards[email][name] = new Board(email, name);
        }

        public void RemoveBoard(string email, string name)
        {
            if (!boards[email].ContainsKey(name))
                throw new ArgumentException("A board with name '" + name + "' does not exsist for user '" + email + "'");
            boards[email].Remove(name);
        }

        public IList<STask> InProgressTasks(string email)
        {
            IList<STask> inProgress = new List<STask>();
            foreach (Board board in boards[email].Values)
            {
                concatLists(inProgress, board.GetColumn(1));
            }
            return inProgress;
        }

        private void concatLists(IList<STask> addTo, IList<STask> addFrom)
        {
            foreach (STask task in addFrom)
                addTo.Add(task);
        }

        private void checkColumnOrdinal(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
                throw new ArgumentException("column ordinal out of range");
        }
    }
}