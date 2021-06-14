using System.Collections.Generic;
using BColumn = IntroSE.Kanban.Backend.BusinessLayer.Column;
using BTask = IntroSE.Kanban.Backend.BusinessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly string Name;
        public readonly int Limit;
        public readonly int Ordinal;
        public readonly IList<string> Tasks;

        internal Column(string Name, int Limit, int Ordinal, IList<string> Tasks)
        {
            this.Name = Name;
            this.Limit = Limit;
            this.Ordinal = Ordinal;
            this.Tasks = Tasks;
        }

        internal Column(BColumn bColumn, int Ordinal)
        {
            Name = bColumn.Name;
            Limit = bColumn.Limit;
            this.Ordinal = Ordinal;
            IList<string> tasks = new List<string>();
            foreach (BTask bTask in bColumn.Tasks)
            {
                tasks.Add(bTask.TaskId + " - " + bTask.Title);
            }
            Tasks = tasks; 
        }
    }
}
