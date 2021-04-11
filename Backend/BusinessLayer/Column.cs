using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class Column
    {
        //fields
        internal readonly string columnName;
        internal int limit;
        private Dictionary<int ,Task> column;

        //constructors
        internal Column(string columnName)
        {
            this.columnName = columnName;
            this.limit = -1;
            this.column = new Dictionary<int, Task>();
        }

        //methods
        internal void limitColumn(int limit)
        {
            if (column.Count < limit)
                throw new ArgumentException(columnName);
            this.limit = limit;
        }

        internal int GetColumnLimit()
        {
            return limit;
        }

        internal Task AddTask(int taskId, DateTime creationTime, string title, string description, DateTime DueDate) {
            if (column.Count == limit)
                throw new OutOfMemoryException(columnName);
            column[taskId] = new Task(taskId, creationTime, title, description, DueDate);
            return column[taskId];
        }

        internal void AddTask(Task task)
        {
            if (column.Count == limit)
                throw new OutOfMemoryException(columnName);
            column[task.TaskId] = task;
        }

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

        internal IList<Task> GetColumn()
        {
            IList<Task> column = new List<Task>;
            foreach (Task task in this.column.Values)
            {
                column.Add(task);
            }
            return column;
        }
    }
}
