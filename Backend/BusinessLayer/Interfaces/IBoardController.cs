using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BusinessLayer.Interfaces
{
    interface IBoardController
    {
        internal void LoadData();
        internal void DeleteData();
        public IList<String> GetBoardNames(string userEmail);
        public IList<String> GetBetterBoardNames(string userEmail);
        internal Board AddBoard(string userEmail, string boardName);
        internal Board JoinBoard(string userEmail, string creatorEmail, string boardName);
        internal void RemoveBoard(string userEmail, string creatorEmail, string boardName);
        public Column AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName);
        public void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName);
        public void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize);
        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal);
        internal void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit);
        internal Task AddTask(string userEmail, string creatorEmail, string boardName, DateTime creationTime, string title, string description, DateTime DueDate);
        internal void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee);
        internal void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime DueDate);
        internal void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title);
        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description);
    }
}
