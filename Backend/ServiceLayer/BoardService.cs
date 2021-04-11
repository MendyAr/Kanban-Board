using System.Collections.Generic;
using System;
using System.Linq;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private BoardController bc;

        internal BoardService()
        {
            bc = new BoardController();
        }

        internal Response LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            throw new NotImplementedException();
        }

        internal Response<int> GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        internal Response<string> GetColumnName(string email, string boardName, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        internal Response<Task> AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        internal Response UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        internal Response UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            throw new NotImplementedException();
        }

        internal Response UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            throw new NotImplementedException();
        }

        internal Response AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            throw new NotImplementedException();
        }

        internal Response<IList<Task>> GetColumn(string email, string boardName, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        internal Response AddBoard(string email, string name)
        {
            throw new NotImplementedException();
        }

        internal Response RemoveBoard(string email, string name)
        {
            throw new NotImplementedException();
        }

        internal Response<IList<Task>> InProgressTasks(string email)
        {
            throw new NotImplementedException();
        }

    }

}
