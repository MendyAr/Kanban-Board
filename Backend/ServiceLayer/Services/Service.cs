using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Service
    {

        //fields
        private readonly UserService UserS;
        private readonly BoardService BoardS;

        //constructors
        public Service()
        {
            BusinessLayer.LoginInstance loginInstance = new BusinessLayer.LoginInstance();
            UserS = new UserService(loginInstance);
            BoardS = new BoardService(loginInstance);
        }

        //methods

        ///<summary>This method loads the data from the persistance.
        ///         You should call this function when the program starts. </summary>
        public Response LoadData()
        {
            Response usersRes = UserS.LoadData();
            Response boardsRes = BoardS.LoadData();
            if (usersRes.ErrorOccured && boardsRes.ErrorOccured)
                return new Response(usersRes.ErrorMessage + "\n" + boardsRes.ErrorMessage);
            if (usersRes.ErrorOccured)
                return usersRes;
            if (boardsRes.ErrorOccured)
                return boardsRes;
            return new Response();
        }

        ///<summary>Removes all persistent data.</summary>
        public Response DeleteData()
        {
            Response usersRes = UserS.DeleteData();
            Response boardsRes = BoardS.DeleteData();
            if (usersRes.ErrorOccured && boardsRes.ErrorOccured)
                return new Response(usersRes.ErrorMessage + "\n" + boardsRes.ErrorMessage);
            if (usersRes.ErrorOccured)
                return usersRes;
            if (boardsRes.ErrorOccured)
                return boardsRes;
            return new Response();
        }

        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="userEmail">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>
        public Response Register(string userEmail, string password)
        {
            return UserS.Register(userEmail, password);
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="userEmail">The userEmail address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string userEmail, string password)
        {
            return UserS.Login(userEmail, password);
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="userEmail">The userEmail of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string userEmail)
        {
            return UserS.Logout(userEmail);
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="userEmail">The userEmail address of the user, must be logged in</param>
        /// /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            return BoardS.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="userEmail">The userEmail address of the user, must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response<Column> res = BoardS.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
            {
                return Response<int>.FromError(res.ErrorMessage);
            }
            return Response<int>.FromValue(res.Value.Limit);
        }

        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="userEmail">The userEmail address of the user, must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response<Column> res = BoardS.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
            {
                return Response<string>.FromError(res.ErrorMessage);
            }
            return Response<string>.FromValue(res.Value.Name);
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="userEmail">userEmail of the user. The user must be logged in.</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            return BoardS.AddTask(userEmail, creatorEmail, boardName, DateTime.Now, title, description, dueDate);
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            return BoardS.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="userEmail">userEmail of user. Must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            return BoardS.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="userEmail">userEmail of user. Must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            return BoardS.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="userEmail">userEmail of user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            return BoardS.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            return BoardS.GetColumnTasks(userEmail, creatorEmail, boardName, columnOrdinal);
        }

        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string userEmail, string name)
        {
            return BoardS.AddBoard(userEmail, name);
        }

        /// <summary>
        /// Adds a board created by another user to the logged-in user. 
        /// </summary>
        /// <param name="userEmail">userEmail of the current user. Must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            return BoardS.JoinBoard(userEmail, creatorEmail, boardName);
        }

        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="creatorEmail">userEmail of the board creator. Must be logged in</param>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            return BoardS.RemoveBoard(userEmail, creatorEmail, boardName);
        }

        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string userEmail)
        {
            return BoardS.InProgressTasks(userEmail);
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userEmail">userEmail of the current user. Must be logged in</param>
        /// <param name="creatorEmail">userEmail of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">userEmail of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            return BoardS.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
        }

        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The userEmail of the user. Must be logged-in.</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(string userEmail)
        {
            return BoardS.GetBoardNames(userEmail);
        }
    }
}
