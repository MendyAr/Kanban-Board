using System.Collections.Generic;
using System;
using BC = IntroSE.Kanban.Backend.BuisnessLayer.BoardController;
using BTask = IntroSE.Kanban.Backend.BuisnessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// Service platform which handles all operations board related
    /// </summary>
    /// <remarks>in method requesting columnOrdinal - the integer will represent 1 out of the 3 columns: 0 - Backlog, 1 - In Progress, 2 - Done</remarks>
    internal class BoardService
    {
        //fields
        private BC bc;

        //constructors
        internal BoardService()
        {
            bc = new BC();
        }

        //methods 

        /// <summary>
        /// registers the user at the board controller
        /// </summary>
        /// <param name="email">the newly registered user's email</param>
        /// <returns>Response containing message detailing the error - if occured</returns>
        internal Response Register(string email)
        {
            try
            {
                bc.Register(email);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// limits specific column in one of the user's boards
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">the user's board name</param>
        /// <param name="columnOrdinal">number representing a column</param>
        /// <param name="limit">new limit</param>
        /// <returns>Response containing message detailing the error - if occured</returns>
        internal Response LimitColumn(string email, string boardName, int columnOrdinal, int limit)
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

        /// <summary>
        /// gets the limit of a specific column in one of the user's boards
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">the user's board name</param>
        /// <param name="columnOrdinal">number representing a column</param>
        /// <returns>Response holding: limit if succeeded, message detailing error if occured</returns>
        internal Response<int> GetColumnLimit(string email, string boardName, int columnOrdinal)
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

        /// <summary>
        /// Returns the column's (represented by columnOrdinal) name of a board if such a board exists
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns>response holding: column name if succeeded, message detailing error if occured</returns>
        internal Response<string> GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                return Response<string>.FromValue(bc.GetColumnName(email, boardName, columnOrdinal);
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Adds new task to a user's board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardName">user's board</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="DueDate">task's due date</param>
        /// <returns>Response holding: the newly added task if succeeded, message detailing the error if occured</returns>
        internal Response<Task> AddTask(string email, string boardName, DateTime creationTime, string title, string description, DateTime DueDate)
        {
            try
            {
                BTask newTask= bc.AddTask(email, boardName, creationTime, title, description, DueDate);
                return Response<Task>.FromValue(new Task(newTask));
            }
            catch (Exception e)
            {
                return Response<Task>.FromError(e.Message);
            }
        }

        /// <summary>
        /// updates an existing task's due date
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="DueDate">new and updated due date</param>
        /// <returns>Response containing message detailing the error if occured</returns>
        internal Response UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime DueDate)
        {
            try
            {
                bc.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId,DueDate);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// updates an existing task's title
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new and updated title</param>
        /// <returns>Response containing message detailing the error if occured</returns>
        internal Response UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
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

        /// <summary>
        /// updates an existing task's description
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new and updated description</param>
        /// <returns>Response containing message detailing the error if occured</returns>
        internal Response UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
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

        /// <summary>
        /// advanced a task to the next column
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">the board in which the task is stored</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <returns>Response containing message detailing the error if occured</returns>
        internal Response AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
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
        
        /// <summary>
        /// returns all the tasks in a specific column of a board
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="boardName">the board in which the column is stored</param>
        /// <param name="columnOrdinal">column name</param>
        /// <returns>Response holding: IList<ServiceLayer.Task> if succeeded, message detailing error if occured</returns>
        internal Response<IList<Task>> GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                IList<BTask> column = bc.GetColumn(email, boardName, columnOrdinal);
                return Response<IList<Task>>.FromValue(translateList(column));
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// creates a new board for the user
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="name">name of the new board</param>
        /// <returns>Reponse containing message detailing the error if occured</returns>
        internal Response AddBoard(string email, string name)
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

        /// <summary>
        /// removes existing board of the user
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <param name="name">removed board's name</param>
        /// <returns>Reponse containing message detailing the error if occured</returns>
        internal Response RemoveBoard(string email, string name)
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

        /// <summary>
        /// Returns all 'In Progress' tasks of a user
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <returns>Response holding: IList containing all of the user's 'In Progress' tasks if succeeded, a message detailing the error if occured</returns>
        internal Response<IList<Task>> InProgressTasks(string email)
        {
            try
            {
                IList<BTask> inProgress = bc.InProgressTasks(email);
                return Response<IList<Task>>.FromValue(translateList(inProgress));
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// translates a list containing tasks of business layer to a list containing the service layer form of the same tasks
        /// </summary>
        /// <param name="originalList">list that will be translated</param>
        /// <returns>IList of service layer tasks</returns>
        private IList<Task> translateList(IList<BTask> originalList)
        {
            IList<Task> translatedList = new List<Task>();
            foreach(BTask bTask in originalList)
            {
                translatedList.Add(new Task(bTask));
            }
            return translatedList;
        }
    }
}
