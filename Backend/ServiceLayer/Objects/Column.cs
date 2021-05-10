﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BColumn = IntroSE.Kanban.Backend.BuisnessLayer.Column;
using BTask = IntroSE.Kanban.Backend.BuisnessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct Column
    {
        public readonly string Name;
        public readonly int Limit;
        public readonly IList<Task> Tasks;

        internal Column(string Name, int Limit, IList<Task> Tasks)
        {
            this.Name = Name;
            this.Limit = Limit;
            this.Tasks = Tasks;
        }

        internal Column(BColumn bColumn)
        {
            this.Name = bColumn.ColumnName;
            this.Limit = bColumn.Limit;
            this.Tasks = translateTasks(bColumn.Tasks);
        }

        private IList<Task> translateTasks(IList<BTask> bTasks)
        {
            IList<Task> tasks = new List<Task>();
            foreach (BTask bTask in bTasks)
            {
                tasks.Add(new Task(bTask));
            }
            return tasks;
        }
    }
}
