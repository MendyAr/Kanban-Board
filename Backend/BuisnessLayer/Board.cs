using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Board
    {
        //fields
        private string name;
        private string creatorEmail;
        private int taskIdCounter = 1;
        private int maxBacklog = -1;
        private int maxInProgress = -1;
        private int maxDone = -1;
        private Dictionary<int,Task> backlog = new Dictionary<int, Task>;
        private Dictionary<int,Task> inProgress = new Dictionary<int, Task>;
        private Dictionary<int,Task> done = new Dictionary<int, Task>;

        //constructor
        public Board(string name, string creatorEmail)
        {
            this.name = name;
            this.creatorEmail = creatorEmail;
            this.id = name + creatorEmail;
        }

        //methods
        private void checkColumnOrdinal(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
                throw new ArgumentException("column ordinal out of range");
        }

        internal void LimitColumn(int columnOrdinal, int limit)
        {
            checkColumnOrdinal;
            if (limit < -1)
                throw new ArgumentException("impossible limit");
            if (columnOrdinal == 0)
            {
                if (backlog.Count > limit)
                    throw new ArgumentException("Impossible to set limit: There are currently more than " + limit + "tasks in column 'Backlog' of this board");
                maxBacklog = limit;
            }
            else if (columnOrdinal == 1)
            {
                if (inProgress.Count > limit)
                    throw new ArgumentException("Impossible to set limit: There are currently more than " + limit + "tasks in column 'In Progress' of this board");
                maxInProgress = limit;
            }
            else
            {
                if (done.Count > limit)
                    throw new ArgumentException("Impossible to set limit: There are currently more than " + limit + "tasks in column 'Done' of this board");
                maxDone = limit;
            }
        }

        internal int GetColumnLimit(int columnOrdinal)
        {
            checkColumnOrdinal;
            if (columnOrdinal == 0)
            {
                return maxBacklog;
            }
            else if (columnOrdinal == 1)
            {
                return maxInProgress;
            }
            else
            {
                return maxDone;
            }
        }
        
        internal Task AddTask(string title, string description, DateTime dueDate)
        {

        }
        
        internal void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
        {
            checkColumnOrdinal;
            if (columnOrdinal == 0)
            {
                try
                {
                    backlog[taskId].setDueDate(dueDate);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board")
                }
            }
            else if (columnOrdinal == 1)
            {
                try
                {
                    inProgress[taskId].setDueDate(dueDate);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board")
                }
            }
            else
            {
                try
                {
                    done[taskId].setDueDate(dueDate);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Done' of this board")
                }
            }

        }

        internal void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
        {
            checkColumnOrdinal;
            if (columnOrdinal == 0)
            {
                try
                {
                    backlog[taskId].setTitle(title);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board")
                }
            }
            else if (columnOrdinal == 1)
            {
                try
                {
                    inProgress[taskId].setTitle(title);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board")
                }
            }
            else
            {
                try
                {
                    done[taskId].setTitle(title);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Done' of this board")
                }
            }

        }
        
        internal void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
        {
            checkColumnOrdinal;
            if (columnOrdinal == 0)
            {
                try
                {
                    backlog[taskId].setDescription(description);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board")
                }
            }
            else if (columnOrdinal == 1)
            {
                try
                {
                    inProgress[taskId].setDescription(description);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board")
                }
            }
            else
            {
                try
                {
                    done[taskId].setDescription(description);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Done' of this board")
                }
            }

        }
        
        internal void AdvanceTask(int columnOrdinal, int taskId)
        {
            checkColumnOrdinal;
            if (columnOrdinal == 0)
            {
                try
                {
                    inProgress[taskId] = backlog[taskId];
                    backlog.Remove(taskId);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board")
                }
            }
            else if (columnOrdinal == 1)
            {
                try
                {
                    done[taskId] = inProgress[taskId];
                    inProgress.Remove(taskId);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board")
                }
            }
            else
            {
                throw new ArgumentException("Cannot advance tasks from column 'Done'")
            }
        }
        
        internal IList<Task> GetColumn(int columnOrdinal)
        {
            checkColumnOrdinal;
            //Ilist<Task> column = new Ilist<Task>;
            if (columnOrdinal == 0)
            {
                foreach (Task task in backlog.Values)
                {
                    column.Add(8);
                }
            }
            else if (columnOrdinal == 1)
            {
                foreach (Task task in backlog.Values)
                {
                    column.Add(8);
                }
            }
            else
            {
                foreach (Task task in backlog.Values)
                {
                    column.Add(8);
                }
            }
            return column;
        }
    }
}
