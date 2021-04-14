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
            log.Info("Starting log!");
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
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>
        public Response Register(string email, string password)
        {
            Response userRegisterResponse = UserS.Register(email, password);
            if (!userRegisterResponse.ErrorOccured)
                userRegisterResponse = BoardS.Register(email);
            return userRegisterResponse;
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            if (connectedEmail != null)
                return Response<User>.FromError("User '" + connectedEmail + "' is currently logged in. Log out before attempting to log in.");
            Response<User> response = UserS.Login(email, password);
            if (!response.ErrorOccured)
            {
                ConnectedEmail = email;
                log.Info("SUCCESSFULLY logged in: '" + email + "'");
            }
            else
            {
                log.Warn("FAILED log in attempt: '" + email + "'");
            }
            return response;
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            if (connectedEmail == null || !ConnectedEmail.Equals(email))
            {
                log.Info("FAILED to logout: '" + ConnectedEmail +"'");
                return new Response("Can't logout: user " + email + " is not logged in");
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
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted LimitColumn(" + email + "," + boardName + "," + columnOrdinal + "," + limit + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.LimitColumn(email, boardName, columnOrdinal, limit);
        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return Response<int>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted GetColumnLimit(" + email + "," + boardName + "," + columnOrdinal + ")");
                return Response<int>.FromError("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.GetColumnLimit(email, boardName, columnOrdinal);
        }

        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return Response<string>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted GetColumnName(" + email + "," + boardName + "," + columnOrdinal + ")");
                return Response<string>.FromError("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.GetColumnName(email, boardName, columnOrdinal);
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return Response<Task>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted AddTask(" + email + "," + boardName + "," + title + "," + description + "," + dueDate + ")");
                return Response<Task>.FromError("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.AddTask(email, boardName, DateTime.Now, title, description, dueDate);
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted UpdateTaskDueDate(" + email + "," + boardName + "," + columnOrdinal + "," + taskId + "," + dueDate + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted UpdateTaskTitle(" + email + "," + boardName + "," + columnOrdinal + "," + taskId + "," + title + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted UpdateTaskDescription(" + email + "," + boardName + "," + columnOrdinal + "," + taskId + "," + description + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted AdvanceTask(" + email + "," + boardName + "," + columnOrdinal + "," + taskId + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.AdvanceTask(email, boardName, columnOrdinal, taskId);
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return Response<IList<Task>>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted GetColumn(" + email + "," + boardName + "," + columnOrdinal + ")");
                return Response<IList<Task>>.FromError("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.GetColumn(email, boardName, columnOrdinal);
        }

        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string email, string name)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted AddBoard(" + email + "," + name + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.AddBoard(email, name);
        }

        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string email, string name)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return new Response("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted RemoveBoard(" + email + "," + name + ")");
                return new Response("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.RemoveBoard(email, name);
        }

        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string email)
        {
            try
            {
                validateLogin(email);
            }
            catch (NullReferenceException)
            {
                return Response<IList<Task>>.FromError("Can't operate -  Please log in first");
            }
            catch (InvalidOperationException)
            {
                log.Warn("OUT OF DOMAIN OPERATION: User '" + ConnectedEmail + "' attempted InProgressTasks(" + email + ")");
                return Response<IList<Task>>.FromError("Can't operate -  User '" + email + "' is not logged in");
            }
            return BoardS.InProgressTasks(email);
        }

        /// <summary>
        /// Validates the user operates legally - i.e. is logged in and operates on his domain
        /// </summary>
        /// <param name="email">calling user's email</param>
        /// <exception cref="NullReferenceException">Thrown if no user is logged in</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user logged in is trying to access data outside his domain</exception>
        private void validateLogin(string email)
        {
            if (connectedEmail == null)
                throw new NullReferenceException();
            if (!email.Equals(connectedEmail))
                throw new InvalidOperationException();
        }
    }
}
