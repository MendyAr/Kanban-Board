using System.Collections.Generic;
using System;
using System.Linq;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private BoardController bc;

        public BoardService()
        {
            bc = new BoardController();
        }

        public Response LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            throw new NotImplementedException();
        }

        public Response<int> GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        public Response<string> GetColumnName(string email, string boardName, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        public Response<Task> AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public Response UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public Response UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            throw new NotImplementedException();
        }

        public Response UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            throw new NotImplementedException();
        }

        public Response AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            throw new NotImplementedException();
        }
        
        public Response<IList<Task>> GetColumn(string email, string boardName, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        public Response AddBoard(string email, string name)
        {
            throw new NotImplementedException();
        }

        public Response RemoveBoard(string email, string name)
        {
            throw new NotImplementedException();
        }

        public Response<IList<Task>> InProgressTasks(string email)
        {
            throw new NotImplementedException();
        }

    }

}
