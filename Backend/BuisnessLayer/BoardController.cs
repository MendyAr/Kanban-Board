using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend

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

        }

        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {

        }

        public Task AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {

        }

        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {

        }

        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {

        }

        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {

        }

        public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {

        }
        
        public iList<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {

        }
        
        public void AddBoard(string email, string name)
        {

        }

        public void RemoveBoard(string email, string name)
        {

        }

        public IList<Task> InProgressTasks(string email)
        {

        }
    }
}