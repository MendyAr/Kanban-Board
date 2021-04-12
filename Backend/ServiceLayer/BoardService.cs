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
            try
            {
                bc.LimitColumn(email, boardName, columnOrdinal, limit);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<int> GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int ColumnLimit= bc.GetColumnLimit(email, boardName, columnOrdinal);
                return Response<int>.FromValue(ColumnLimit);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

        public Response<string> GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                IList<BuisnessLayer.Task> column = bc.GetColumn(email, boardName, columnOrdinal);
                return Response<string>.FromValue();
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<Task> AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                 BuisnessLayer.Task newTask= bc.AddTask(email, boardName,DateTime.Now,title,description,dueDate);
                Task sTask = new Task(newTask.TaskId,newTask.CreationTime,newTask.Title,newTask.Description,newTask.DueDate);
                return Response<Task>.FromValue(sTask);
            }
            catch (Exception e)
            {
                return Response<Task>.FromError(e.Message);
            }
        }

        public Response UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                bc.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId,dueDate);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                bc.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                bc.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                bc.AdvanceTask(email, boardName, columnOrdinal, taskId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        
        public Response<IList<Task>> GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                IList<BuisnessLayer.Task> column = bc.GetColumn(email, boardName, columnOrdinal);

                IList<Task> Scolumn = 
                Task sTask = new Task(newTask.TaskId, newTask.CreationTime, newTask.Title, newTask.Description, newTask.DueDate);
                return Response<Task>.FromValue(sTask);
            }
            catch (Exception e)
            {
                return Response<Task>.FromError(e.Message);
            }
        }

        public Response AddBoard(string email, string name)
        {
            try
            {
                bc.AddBoard(email, name);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response RemoveBoard(string email, string name)
        {
            try
            {
                bc.RemoveBoard(email, name);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<IList<Task>> InProgressTasks(string email)
        {
            throw new NotImplementedException();
        }

    }

}
