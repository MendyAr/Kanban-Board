using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.Interfaces
{
    interface IBoard
    {
        internal void AddMember(string memberEmail);
        public Column AddColumn(string creatorEmail, string boardName, int columnOrdinal, string columnName);
        public void RenameColumn(int columnOrdinal, string newColumnName);
        public void MoveColumn(int columnOrdinal, int shiftSize);
        public void RemoveColumn(int columnOrdinal);
        internal void LimitColumn(int columnOrdinal, int limit);
        internal Task AddTask(DateTime creationTime, string title, string description, DateTime dueDate, string assignee, string boardCreator, string boardName);
        internal void AssignTask(string userEmail, int columnOrdinal, int taskId, string assignee);
        internal void UpdateTaskDueDate(string userEmail, int columnOrdinal, int taskId, DateTime DueDate);
        internal void UpdateTaskTitle(string userEmail, int columnOrdinal, int taskId, string title);
        internal void UpdateTaskDescription(string userEmail, int columnOrdinal, int taskId, string description);
        internal void AdvanceTask(string userEmail, int columnOrdinal, int taskId);
        internal Column GetColumn(int columnOrdinal);
        internal void checkColumnOrdinal(int columnOrdinal);
        internal IList<Task> GetColumnTasks(int columnOrdinal);
        internal Column AddColumn(Column column);
    }
}
