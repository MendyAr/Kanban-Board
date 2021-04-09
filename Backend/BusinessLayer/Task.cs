using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Task //i think we can make it struct
    {
        public int taskId;
        public DateTime creationTime;
        public DateTime dueDate;
        public string title;
        public string description;

    }
}
