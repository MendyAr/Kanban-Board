using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Board
    {
        //fields
        private int taskIdCounter = 1;
        private Column[] columns;

        //constructor
        internal Board()
        {
            this.columns = new Column[] { new Column("Backlog"), new Column("In Progress"), new Column("Done")};
        }

        //methods
        internal void LimitColumn(int columnOrdinal, int limit)
        {
            columns[columnOrdinal].limitColumn(limit);
        }

        internal int GetColumnLimit(int columnOrdinal)
        {
            return columns[columnOrdinal].GetColumnLimit();
        }
        
        internal Task AddTask(DateTime creationTime, string title, string description, DateTime DueDate)
        {
            Task task = columns[0].AddTask(taskIdCounter, creationTime, title, description, DueDate);
            taskIdCounter++;
            return task;
        }
        
        internal void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime DueDate)
        {
            columns[columnOrdinal].UpdateTaskDueDate(taskId, DueDate);
        }

        internal void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
        {
            columns[columnOrdinal].UpdateTaskTitle(taskId, title);
        }
        
        internal void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
        {
            columns[columnOrdinal].UpdateTaskDescription(taskId, description);
        }
        
        internal void AdvanceTask(int columnOrdinal, int taskId)
        {
            Task task = columns[columnOrdinal].RemoveTask(taskId);
            try
            {
                columns[columnOrdinal + 1].AddTask(task);
            }
            catch (OutOfMemoryException e)
            {
                columns[columnOrdinal].AddTask(task);
                throw new OutOfMemoryException(e.Message);
            }
        }
        
        internal IList<Task> GetColumn(int columnOrdinal)
        {
            return columns[columnOrdinal].GetColumn();
        }
    }
}
