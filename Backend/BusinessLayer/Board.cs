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
        private Dictionary<int,Task> backlog = new Dictionary<int, Task>();
        private Dictionary<int,Task> inProgress = new Dictionary<int, Task>();
        private Dictionary<int,Task> done = new Dictionary<int, Task>();

        //constructor
        internal Board(string creatorEmail, string name)
        {
            this.name = name;
            this.creatorEmail = creatorEmail;
        }

        //methods
        internal void LimitColumn(int columnOrdinal, int limit)
        {
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
        
        internal Task AddTask(DateTime creationTime, string title, string description, DateTime dueDate)
        {
            if (maxBacklog != -1 && backlog.Count == maxBacklog)
                throw new IndexOutOfRangeException("Cannot add new tasks into board '" + this.creatorEmail + ":" + this.name + "': 'Backlog' column is already at its limit");
            Task task = new Task(taskIdCounter, creationTime, title, description, dueDate);
            backlog[taskIdCounter] = task;
            taskIdCounter++;
            return task;
        }
        
        internal void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (columnOrdinal == 0)
            {
                try
                {
                    backlog[taskId].setDueDate(dueDate);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board");
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
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board");
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
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Done' of this board");
                }
            }

        }

        internal void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
        {
            if (columnOrdinal == 0)
            {
                try
                {
                    backlog[taskId].setTitle(title);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board");
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
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board");
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
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Done' of this board");
                }
            }

        }
        
        internal void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
        {
            if (columnOrdinal == 0)
            {
                try
                {
                    backlog[taskId].setDescription(description);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board");
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
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board");
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
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Done' of this board");
                }
            }

        }
        
        internal void AdvanceTask(int columnOrdinal, int taskId)
        {
            if (columnOrdinal == 0)
            {
                if (maxInProgress != -1 && inProgress.Count == maxInProgress)
                    throw new IndexOutOfRangeException("Cannot advance tasks from 'Backlog': 'In Progress' column is already at its limit");
                try
                {
                    inProgress[taskId] = backlog[taskId];
                    backlog.Remove(taskId);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'Backlog' of this board");
                }
            }
            else
            {
                if (maxDone != -1 && done.Count == maxDone)
                    throw new IndexOutOfRangeException("Cannot advance tasks from 'In Progress': 'Done' column is already at its limit");
                try
                {
                    done[taskId] = inProgress[taskId];
                    inProgress.Remove(taskId);
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("There is no task with id '" + taskId + "' in column 'In Progress' of this board");
                }
            }
        }
        
        internal IList<Task> GetColumn(int columnOrdinal)
        {
            IList<Task> column = new List<Task>();
            if (columnOrdinal == 0)
            {
                foreach (Task task in backlog.Values)
                {
                    column.Add(task);
                }
            }
            else if (columnOrdinal == 1)
            {
                foreach (Task task in backlog.Values)
                {
                    column.Add(task);
                }
            }
            else
            {
                foreach (Task task in backlog.Values)
                {
                    column.Add(task);
                }
            }
            return column;
        }
    }
}
