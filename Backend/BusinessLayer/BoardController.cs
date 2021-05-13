using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// A collection of all the boards in the system. Manages operationg within specific boards and general board related checks
    /// </summary>
    /// <remarks>in methods requesting columnOrdinal - the integer will represent 1 out of the 3 columns: 0 - Backlog, 1 - In Progress, 2 - Done</remarksY>
    internal class BoardController
    {
        //fields
        private Dictionary<string, Dictionary<string, Board>> boards; //first key is userEmail , second key will be the board name
        private Dictionary<string, HashSet<string>> userBoards; //first key is userEmail, second key is a set of all the boards he is a member of
        private LoginInstance loginInstance;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //constructors
        internal BoardController(LoginInstance loginInstance)
        {
            boards = new Dictionary<string, Dictionary<string, Board>>();
            this.loginInstance = loginInstance;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        //methods

        /// <summary>
        /// creates a new entry for a newly registered user
        /// </summary>
        /// <param name="userEmail">the newly registered user's email</param>
        internal void Register(string userEmail)
        {
            boards[userEmail] = new Dictionary<string, Board>();
            userBoards[userEmail] = new HashSet<string>();
            log.Info($"SUCCESSFULLY created new entry for user: '{userEmail}'");
        }

        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <returns>IList detailing all the board which the user is a member of</returns>
        /// <remarks>call validateLogin</remarks>
        public IList<String> GetBoardNames(string userEmail)
        {
            validateLogin(userEmail, $"GetBoardNames({userEmail})");
            List<String> boards = new List<string>();
            foreach(String board in userBoards[userEmail])
            {
                boards.Add(board);
            }
            return boards;
        }

        /// <summary>
        /// adds a new board for a user
        /// </summary>
        /// <param name="userEmail">the calling user's email</param>
        /// <param name="boardName">the new board name</param>
        /// <exception cref="ArgumentException">thrown when the board already exists for that user</exception>
        /// <remarks>call validateLogin</remarks>
        internal void AddBoard(string userEmail, string boardName)
        {
            validateLogin(userEmail, $"AddBoard({userEmail}, {boardName})");
            if (boards[userEmail].ContainsKey(boardName))
            {
                log.Warn($"FAILED to create board: '{userEmail}:{boardName}' - already exists");
                throw new ArgumentException($"Board '{userEmail}:{boardName}' already exist");
            }
            boards[userEmail][boardName] = new Board();
            JoinBoard(userEmail, userEmail, boardName);
            log.Info($"SUCCESSFULLY created '{userEmail}:{boardName}'");
        }

        /// <summary>
        /// Signs the user to an existing board
        /// </summary>
        /// <param name="userEmail">the calling user's email</param>
        /// <param name="creatorEmail">board's creator - idetifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <remarks>calls validateLogin</remarks>
        internal void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            validateLogin(userEmail, $"JoinBoard({userEmail}, {creatorEmail}, {boardName})");
            if (userBoards[userEmail].Contains($"{creatorEmail}:{boardName}"))
            {
                throw new Exception($"{userEmail} already memeber of {creatorEmail}:{boardName}");
            }
            userBoards[userEmail].Add($"{creatorEmail}:{boardName}");
            log.Info($"SUCCESSFULLY signed '{userEmail}' to '{creatorEmail}:{boardName}'");
        }


        /// <summary>
        /// deletes an existing board
        /// </summary>
        /// <param name="email">the calling user's email</param>
        /// <param name="boardName">the deleted board's name</param>
        /// <exception cref="ArgumentException">thrown when trying to delete a non-existing board</exception>
        /// <remarks>calls validateLogin, checkBoardExistance</remarks>
        internal void RemoveBoard(string userEmail, string boardName)
        {
            validateLogin(userEmail, $"RemoveBoard({userEmail}, {boardName})");
            checkBoardExistance(userEmail, boardName, "remove");
            boards[userEmail].Remove(boardName);
            log.Info($"SUCCESSFULLY removed '{userEmail}:{boardName}'");
        }

        /// <summary>
        /// sets a limit to a specific column in a specific board
        /// </summary>
        /// <param name="userEmail">the calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">the column</param>
        /// <param name="limit">new and updated limit</param>
        /// <exception cref="ArgumentException">thrown if the new limit isn't legal, if it's impossible to set the limit due to column complications</exception>
        /// <remarks>calls validateLogin, checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            validateLogin(userEmail, $"LimitColumn({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {limit})");
            checkMembership(userEmail, creatorEmail, boardName, "LimitColumn");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            if (limit < -1)
            {
                log.Warn($"FAILED to set an impossible limit to '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}'. Limit: " + limit);
                throw new ArgumentException("impossible limit");
            }
            try
            {
                boards[creatorEmail][boardName].LimitColumn(columnOrdinal, limit);
                log.Info($"SUCCESSFULLY set new limit for '{creatorEmail}:{boardName}'[{columnOrdinal}]' by '{userEmail}'. Limit: " + limit);
            }
            catch (ArgumentException e)
            {
                log.Warn($"FAILED to set limit for '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Column has more than " + limit + " tasks. Limit: " + limit);
                throw new ArgumentException($"Cannot set limit: There are more than {limit} tasks in column '{e.Message}' of board '{creatorEmail}:{boardName}'");
            }
        }

        /// <summary>
        /// adds a new task to a board (always adds the task to 'Backlog' column)
        /// </summary>
        /// <param name="userEmail">the calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="creationTime">task's creation time</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="DueDate">task's due date</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">throw if one of the task's arguments isn't legal</exception>
        /// <exception cref="OutOfMemoryException">thrown if the column is already at its limit</exception>
        /// <remarks>calls validateLogin, checkMembership, checkBoardExistance</remarks>
        internal Task AddTask(string userEmail, string creatorEmail, string boardName, DateTime creationTime, string title, string description, DateTime DueDate)
        {
            validateLogin(userEmail, $"AddTask({userEmail}, {creatorEmail}, {boardName}, {title}");
            checkMembership(userEmail, creatorEmail, boardName, "AddTask");
            checkBoardExistance(creatorEmail, boardName, "access");
            try
            {
                Task task = boards[creatorEmail][boardName].AddTask(userEmail, creationTime, title, description, DueDate);
                log.Info($"SUCCESSFULLY added task '{task.TaskId}' to '{creatorEmail}:{boardName}' by '{userEmail}'");
                return task;
            }
            catch (OutOfMemoryException e)
            {
                log.Warn($"FAILED to add task '{title}' to '{creatorEmail}:{boardName}[{0}]' by '{userEmail}' - Column is at it's limit");
                throw new OutOfMemoryException($"Cannot add task '{title}': Column '{e.Message}' of board '{creatorEmail}:{boardName}' is currently at its limit");
            }
            catch (Exception e)
            {
                log.Warn($"FAILED to add task '{title}' to '{creatorEmail}:{boardName}[{0}]' by '{userEmail}' - Crashed at Task: {e.Message}");
                throw new ArgumentException($"Cannot add task '{title}': {e.Message}");
            }
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">userEmail of the user to assign to task to</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if asked to update a task from column 2</exception>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new DueDate isn't legal</exception>
        /// <remarks>calls validateLogin, checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            validateLogin(userEmail, $"AssignTask({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {taskId}, {emailAssignee})");
            checkMembership(userEmail, creatorEmail, boardName, "AssignTask");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            if (columnOrdinal == 2)
            {
                log.Warn($"FAILED to assign task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Updating tasks at column 'done' is prohibited");
                throw new ArgumentOutOfRangeException("Cannot reassign tasks in column 'done'");
            }
            try
            {
                boards[creatorEmail][boardName].AssignTask(userEmail, columnOrdinal, taskId, emailAssignee);
                log.Info($"SUCCESSFULLY assigned Task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' to '{emailAssignee}' by '{userEmail}'");
            }
            catch (IndexOutOfRangeException e)
            {
                log.Warn($"FAILED to assign task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' to '{emailAssignee}' by '{userEmail}' - Task not found");
                throw new ArgumentException($"Cannot assign task: A task with ID '{taskId}' does not exist in column '{e.Message}' of board '{creatorEmail}:{boardName}'");
            }
            catch (Exception e)
            {
                log.Warn($"FAILED to assign task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' to '{emailAssignee}' by '{userEmail}' - Failed at Task: {e.Message}");
                throw new ArgumentException($"Cannot update task: {e.Message}");
            }
        }

        /// <summary>
        /// updates an existing task's due date
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="DueDate">new and updated due date</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if asked to update a task from column 2</exception>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new DueDate isn't legal</exception>
        /// <remarks>calls validateLogin, checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime DueDate)
        {
            validateLogin(userEmail, $"UpdateTaskDueDate({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {taskId}, {DueDate})");
            checkMembership(userEmail, creatorEmail, boardName, "UpdateTaskDueDate");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            if (columnOrdinal == 2)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Updating tasks at column 'done' is prohibited");
                throw new ArgumentOutOfRangeException("Cannot update tasks in column 'done'");
            }
            try
            {
                boards[creatorEmail][boardName].UpdateTaskDueDate(userEmail, columnOrdinal, taskId, DueDate);
                log.Info($"SUCCESSFULLY updated Task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - DueDate");
            }
            catch (IndexOutOfRangeException e)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Task not found");
                throw new ArgumentException($"Cannot update task: A task with ID '{taskId}' does not exist in column '{e.Message}' of board '{creatorEmail}:{boardName}'");
            }
            catch (Exception e)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Failed at Task: {e.Message}");
                throw new ArgumentException($"Cannot update task: {e.Message}");
            }
        }

        /// <summary>
        /// updates an existing task's title
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new and updated title</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if asked to update a task from column 2</exception>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new title isn't legal</exception>
        /// <remarks>calls validateLogin, checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            validateLogin(userEmail, $"UpdateTaskTitle({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {taskId}, {title})");
            checkMembership(userEmail, creatorEmail, boardName, "UpdateTaskTitle");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            if (columnOrdinal == 2)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Updating tasks at column 'done' is prohibited");
                throw new ArgumentOutOfRangeException("Cannot update tasks in column 'done'");
            }
            try
            {
                boards[creatorEmail][boardName].UpdateTaskTitle(userEmail, columnOrdinal, taskId, title);
                log.Info($"SUCCESSFULLY updated Task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Title");
            }
            catch (IndexOutOfRangeException e)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Task not found");
                throw new ArgumentException($"Cannot update task: A task with ID '{taskId}' does not exist in column '{e.Message}' of board '{creatorEmail}:{boardName}'");
            }
            catch (Exception e)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Failed at Task: {e.Message}");
                throw new ArgumentException($"Cannot update task: {e.Message}");
            }
        }

        /// <summary>
        /// updates an existing task's description
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new and updated description</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if asked to update a task from column 2</exception>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if new description isn't legal</exception>
        /// <remarks>calls validateLogin checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            validateLogin(userEmail, $"UpdateTaskDescription({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {taskId}, {description})");
            checkMembership(userEmail, creatorEmail, boardName, "UpdateTaskDescription");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            if (columnOrdinal == 2)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{userEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Updating tasks at column 'done' is prohibited");
                throw new ArgumentOutOfRangeException("Cannot update tasks in column 'done'");
            }
            try
            {
                boards[creatorEmail][boardName].UpdateTaskDescription(userEmail, columnOrdinal, taskId, description);
                log.Info($"SUCCESSFULLY updated Task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Description");
            }
            catch (IndexOutOfRangeException e)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Task not found");
                throw new ArgumentException($"Cannot update task: A task with ID '{taskId}' does not exist in column '{e.Message}' of board '{creatorEmail}:{boardName}'");
            }
            catch (Exception e)
            {
                log.Warn($"FAILED to update task '{taskId}' at '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Failed at Task: {e.Message}");
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// advanced an existing task's to the next column
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">column in which the task is stored</param>
        /// <param name="taskId">task's ID</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if asked to advance a task from column 2</exception>
        /// <exception cref="ArgumentException">Throw if the task isn't stored in said column, if next column is at it's limit</exception>
        /// <remarks>calls validateLogin, checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            validateLogin(userEmail, $"AdvanceTask({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal}, {taskId})");
            checkMembership(userEmail, creatorEmail, boardName, "AdvanceTask");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            if (columnOrdinal == 2)
            {
                log.Warn($"FAILED to advance task '{taskId}' from '{userEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Advancing tasks from column 'done' is prohibited");
                throw new ArgumentOutOfRangeException("Cannot advance tasks from column 'done'");
            }
            try
            {
                boards[creatorEmail][boardName].AdvanceTask(userEmail, columnOrdinal, taskId);
                log.Info($"SUCCESSFULLY advanced Task '{taskId}' from '{creatorEmail}:{boardName}[{columnOrdinal}]' to '{creatorEmail}:{boardName}[{columnOrdinal + 1}]' by '{userEmail}'");
            }
            catch (IndexOutOfRangeException e)
            {
                log.Warn($"FAILED to advance task '{taskId}' from '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Task not found");
                throw new ArgumentException($"Cannot advance task: A task with ID '{taskId}' does not exist in column '{e.Message}' of board '{creatorEmail}:{boardName}'");
            }
            catch (OutOfMemoryException e)
            {
                log.Warn($"FAILED to advance task '{taskId}' from '{creatorEmail}:{boardName}[{columnOrdinal}]' by '{userEmail}' - Column '{creatorEmail}:{boardName}[{columnOrdinal + 1}]' is currently at its limit");
                throw new OutOfMemoryException($"Cannot advance task: Column '{e.Message}' of board '{creatorEmail}:{boardName}' is currently at its limit");
            }
        }

        /// <summary>
        /// Finds and returns specific column of a specific board
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">column index</param>
        /// <returns>Requested column</returns>
        /// <remarks>calls checkMembership, checkColumnOrdinal, checkBoardExistance</remarks>
        internal Column GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            validateLogin(userEmail, $"GetColumn({userEmail}, {creatorEmail}, {boardName}, {columnOrdinal})");
            checkMembership(userEmail, creatorEmail, boardName, "GetColumn");
            checkBoardExistance(creatorEmail, boardName, "access");
            checkColumnOrdinal(creatorEmail, boardName, columnOrdinal);
            return boards[userEmail][boardName].GetColumn(columnOrdinal);
        }

        /// <summary>
        /// gets all the tasks of a user that are 'In Progress'
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <returns>IList of the user's 'In Progress' tasks</returns>
        /// <remarks>calls validateLogin, concatLists</remarks>
        internal IList<Task> InProgressTasks(string userEmail)
        {
            validateLogin(userEmail, $"InProgressTasks({userEmail})");
            IList<Task> inProgress = new List<Task>();
            foreach (String board in userBoards[userEmail])
            {
                string[] boardDetails = board.Split(':');
                concatLists(inProgress, boards[boardDetails[0]][boardDetails[1]].GetColumn(1));
            }
            return inProgress;
        }

        /// <summary>
        /// adds the content of one list to the other
        /// </summary>
        /// <param name="addTo">the list that the items will be added to</param>
        /// <param name="addFrom">the list that the items will be added from</param>
        private void concatLists(IList<Task> addTo, IList<Task> addFrom)
        {
            foreach (Task task in addFrom)
                addTo.Add(task);
        }

        /// <summary>
        /// checks legality of columnOrdinal
        /// </summary>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="columnOrdinal">columnOrdinal that needs to be checked</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the given ordinal isn't legal</exception>
        private void checkColumnOrdinal(string creatorEmail, string boardName, int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                log.Warn($"Failed to access '{creatorEmail}:{boardName}[{columnOrdinal}]' - Column doesn't exist");
                throw new IndexOutOfRangeException("Column ordinal out of range: Argument needs to be between 0 and 2 (inclusive)");
            }
        }

        /// <summary>
        /// checks whether a board exists or not
        /// </summary>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="action">the action that was tried</param>
        /// <exception cref="ArgumentException">thrown if the board does not exist</exception>
        private void checkBoardExistance(string creatorEmail, string boardName, string action)
        {
            if (creatorEmail == null || boardName == null)
            {
                log.Warn($"FAILED to {action} '{creatorEmail}:{boardName}' - email and boardName cannot be null");
            }
            if (!boards[creatorEmail].ContainsKey(boardName))
            {
                log.Warn($"FAILED to {action} '{creatorEmail}:{boardName}' - Board doesn't exist");
                throw new ArgumentException($"Board '{creatorEmail}:{boardName}' does not exist");
            }
        }

        /// <summary>
        /// checks whether a user is a member of given board
        /// </summary>
        /// <param name="userEmail">calling user's email</param>
        /// <param name="creatorEmail">board's creator - identifier</param>
        /// <param name="boardName">board's name - identifier</param>
        /// <param name="method">the method calling this check</param>
        /// <exception cref="AccessViolationException">thrown if the user is trying to access a board which he is not a member of</exception>
        private void checkMembership(string userEmail, string creatorEmail, string boardName, string method)
        {
            if (!userBoards[userEmail].Contains($"{creatorEmail}:{boardName}")) {
                log.Warn($"ACCESS VIOLATION - '{method}' - {userEmail} is not a member of '{creatorEmail}:{boardName}'");
                throw new AccessViolationException($"{userEmail} is not a member of '{creatorEmail}:{boardName}'");
            }
        }

        private void validateLogin(string userEmail, string attempt)
        {
            try
            {
                loginInstance.ValidateLogin(userEmail);
            }
            catch (InvalidOperationException e)
            {
                log.Warn($"OUT OF DOMAIN OPERATION: User '{loginInstance.ConnectedEmail}' attempted '{attempt}'");
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}