using System.Collections.Generic;
using BColumn = IntroSE.Kanban.Backend.BusinessLayer.Column;
using BTask = IntroSE.Kanban.Backend.BusinessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly string Name;
        public readonly int Limit;
        public readonly IList<string> Tasks;

        internal Column(string Name, int Limit, IList<string> Tasks)
        {
            this.Name = Name;
            this.Limit = Limit;
            this.Tasks = Tasks;
        }

        internal Column(BColumn bColumn)
        {
            Name = bColumn.Name;
            Limit = bColumn.Limit;
            IList<string> tasks = new List<string>();
            foreach (BTask bTask in bColumn.Tasks)
            {
                tasks.Add(bTask.TaskId + " - " + bTask.Title);
            }
            Tasks = tasks; 
        }
    }
}
