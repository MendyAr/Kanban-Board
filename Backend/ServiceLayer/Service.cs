using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Service
    {

        //fields
        private readonly UserService UserS;
        private readonly BoardService BoardS;
        private string connectedEmail;
        public string ConnectedEmail { get => connectedEmail; set => connectedEmail = value; }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //constructors
        public Service()
        {
            //LoadData();
            UserS = new UserService();
            BoardS = new BoardService();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Kanban.app booted");
        }

        //methods

        ///<summary>This method loads the data from the persistance.
        ///         You should call this function when the program starts. </summary>
        public Response LoadData()
        {
            throw new NotImplementedException();
        }

        ///<summary>Removes all persistent data.</summary>
        public Response DeleteData()
        {
            throw new NotImplementedException();
        }

        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="userEmail">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>
        public Response Register(string userEmail, string password)
        {
            Response userRegisterResponse = UserS.Register(userEmail, password);
            if (!userRegisterResponse.ErrorOccured)
                userRegisterResponse = BoardS.Register(userEmail);
            return userRegisterResponse;
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="userEmail">The userEmail address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string userEmail, string password)
        {
            if (connectedEmail != null)
                return Response<User>.FromError("User '" + connectedEmail + "' is currently logged in. Log out before attempting to log in.");
            Response<User> response = UserS.Login(userEmail, password);
            if (!response.ErrorOccured)
            {
                ConnectedEmail = userEmail;
                log.Info("SUCCESSFULLY logged in: '" + userEmail + "'");
            }
            else
            {
                log.Warn("FAILED log in attempt: '" + userEmail + "'");
            }
            return response;
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="userEmail">The userEmail of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string userEmail)
        {
            if (connectedEmail == null || !ConnectedEmail.Equals(userEmail))
            {
                log.Info("FAILED to logout: '" + ConnectedEmail +"'");
                return new Response("Can't logout: user " + userEmail + " is not logged in");
            }
            else
            {
                log.Info("SUCCESSFULLY logged out: '" + ConnectedEmail +"'");
                ConnectedEmail = null;
                return new Response();
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted LimitColumn(" + userEmail + "," + boardName + "," + columnOrdinal + "," + limit + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return Response<int>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted GetColumnLimit(" + userEmail + "," + boardName + "," + columnOrdinal + ")");
                return Response<int>.FromError("Can't operate -  User '" + userEmail + "' is not logged in");
            }
            return BoardS.GetColumnLimit(userEmail, creatorEmail, boardName, columnOrdinal);
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return Response<string>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted GetColumnName(" + userEmail + "," + boardName + "," + columnOrdinal + ")");
                return Response<string>.FromError("Can't operate -  User '" + userEmail + "' is not logged in");
            }
            return BoardS.GetColumnName(userEmail, creatorEmail, boardName, columnOrdinal);
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return Response<Task>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted AddTask(" + userEmail + "," + boardName + "," + title + "," + description + "," + dueDate + ")");
                return Response<Task>.FromError("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted UpdateTaskDueDate(" + userEmail + "," + boardName + "," + columnOrdinal + "," + taskId + "," + dueDate + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted UpdateTaskTitle(" + userEmail + "," + boardName + "," + columnOrdinal + "," + taskId + "," + title + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted UpdateTaskDescription(" + userEmail + "," + boardName + "," + columnOrdinal + "," + taskId + "," + description + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted AdvanceTask(" + userEmail + "," + boardName + "," + columnOrdinal + "," + taskId + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return Response<IList<Task>>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted GetColumn(" + userEmail + "," + boardName + "," + columnOrdinal + ")");
                return Response<IList<Task>>.FromError("Can't operate -  User '" + userEmail + "' is not logged in");
            }
            return BoardS.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal);
        }

        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string userEmail, string name)
        {
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted AddBoard(" + userEmail + "," + name + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn($"OUT OF DOMAIN OPERATION: User '{ConnectedEmail}' attempted JoinBoard({userEmail}, {creatorEmail}, {boardName})");
                return new Response($"Can't operate -  User '{userEmail}' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted RemoveBoard(" + userEmail + "," + boardName + ")");
                return new Response("Can't operate -  User '" + userEmail + "' is not logged in");
            }
            return BoardS.RemoveBoard(userEmail, creatorEmail, boardName);
        }

        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="userEmail">userEmail of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string userEmail)
        {
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return Response<IList<Task>>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn($"OUT OF DOMAIN OPERATION: User '{ConnectedEmail}' attempted InProgressTasks({userEmail})");
                return Response<IList<Task>>.FromError($"Can't operate -  User '{userEmail}' is not logged in");
            }
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
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn($"OUT OF DOMAIN OPERATION: User '{ConnectedEmail}' attempted AssignTask({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {taskId}, {emailAssignee})");
                return new Response($"Can't operate -  User '{userEmail}' is not logged in");
            }
            return BoardS.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
        }

        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The userEmail of the user. Must be logged-in.</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(string userEmail)
        {
            try
            {
                validateLogin(userEmail);
            }
            catch (NullReferenceException)
            {
                return Response<IList<String>>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn($"OUT OF DOMAIN OPERATION: User '{ConnectedEmail}' attempted GetBoardNames({userEmail})");
                return Response<IList<String>>.FromError($"Can't operate -  User '{userEmail}' is not logged in");
            }
            return BoardS.GetBoardNames(userEmail);
        }

        /// <summary>
        /// Validates the user operates legally - i.e. is logged in and operates on his domain
        /// </summary>
        /// <param name="userEmail">calling user's userEmail</param>
        /// <exception cref="NullReferenceException">Thrown if no user is logged in</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user logged in is trying to access data outside his domain</exception>
        private void validateLogin(string userEmail)
        {
            if (connectedEmail == null)
                throw new NullReferenceException();
            if (!userEmail.Equals(connectedEmail))
                throw new InvalidOperationException();
        }
    }
}
