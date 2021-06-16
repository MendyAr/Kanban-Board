using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.Interfaces
{
    interface IColumn
    {
        internal Task AddTask(int taskId, DateTime creationTime, string title, string description, DateTime dueDate, string assignee, string boardCreator, string boardName);
        internal void AddTask(Task task);
        internal void AddTasks(IList<Task> tasks);
        internal Task RemoveTask(string userEmail, int taskId);
        internal void AssignTask(string userEmail, int taskId, string assignee);
        internal void UpdateTaskDueDate(string userEmail, int taskId, DateTime DueDate);
        internal void UpdateTaskTitle(string userEmail, int taskId, string title);
        internal void UpdateTaskDescription(string userEmail, int taskId, string description);
        internal void UpdateOrdinal(int columnOrdinal);
        internal void validateAssignee(string userEmail, int taskId);
        internal Column AddColumn(Column column);
    }
}
