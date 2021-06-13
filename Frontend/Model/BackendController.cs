using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer;
using STask = IntroSE.Kanban.Backend.ServiceLayer.Task;
using SUser = IntroSE.Kanban.Backend.ServiceLayer.User;
namespace IntroSE.Kanban.Frontend.Model
{
    public class BackendController
    {
        private Service Service { get; set; }
        public BackendController(Service service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new Service();
            Service.LoadData();
        }

        public User Login(string username, string password)
        {
            Response<SUser> user = Service.Login(username, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            string email = user.Value.Email;
            Collection <string> boards = new Collection<string>(GetBetterBoardNames(email));
            return new User(email,boards);
        }

        internal void Register(string username, string password)
        {
            Response res = Service.Register(username, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        ///<summary>Removes all persistent data.</summary>
        public void DeleteData()
        {
            Service.DeleteData();
        }


        internal void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            Response res = Service.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            Response res = Service.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            Response res = Service.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response res = Service.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal Task AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            Response<STask> res = Service.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new Task(res.Value);
        }

        internal void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = Service.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response res = Service.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response res = Service.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response res = Service.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal IList<Task> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response<IList<STask>> res = Service.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else
            {
                IList<Task> tasks = new List<Task>();
                foreach (STask s_task in res.Value)
                {
                    tasks.Add(new Task(s_task));
                }
                return tasks;
            }
        }

        public void AddBoard(string userEmail, string name)
        {
            Response res = Service.AddBoard(userEmail, name);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            Response res = Service.JoinBoard(userEmail, creatorEmail, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            Response res = Service.RemoveBoard(userEmail, creatorEmail, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal IList<Task> GetInProgressTasks(string userEmail)
        {
            Response<IList<STask>> res = Service.InProgressTasks(userEmail);
            
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else
            {
                IList<Task> tasks = new List<Task>();
                foreach (STask s_task in res.Value)
                {
                    tasks.Add(new Task(s_task));
                }
                return tasks;
            }
        }

        internal void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response res = Service.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal IList<String> GetBoardNames(string userEmail)
        {
            Response<IList<String>> res = Service.GetBoardNames(userEmail);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            else
            {
                IList<String> board_names = new List<String>();
                foreach (String name in res.Value)
                {
                    board_names.Add(name);
                }
                return board_names;
            }
        }

        internal IList<string> GetBetterBoardNames(string email)
        {
            return Service.GetBetterBoardNames(email).Value;
        }
    }
}
