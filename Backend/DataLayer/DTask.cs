using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTask : DTO
    {
        
        private readonly int taskId;
        public int TaskId
        { get { return taskId; } }


        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (Persist)
                {
                    Update("Title", value);
                }
                title = value;
            }
        }


        private string description;
        public string Description
        {
            get { return description; }
            set
            {

                if (Persist)
                {
                   Update("Description", value);
                }
                description = value;
            }
        }


        private readonly DateTime creationTime;
        public DateTime CreationTime { get { return creationTime; } }


        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {

                if (Persist)
                {
                    Update("DueDate", value.ToString());
                }
                dueDate = value;
            }
        }


        private string assignee;
        public string Assignee { get { return assignee; } 
            set
            {
                if (Persist)
                {
                    Update("Assignee", value);
                }
                assignee = value;
            } }

        private string _boardCreator;
        public string BoardCreator { get => _boardCreator; set
            {
                if (Persist)
                {
                    Update("BoardCreator", value);
                }
                _boardCreator = value;
            } }

        private string _boardName;
        public string BoardName
        {
            get => _boardName; set
            {
                if (Persist)
                {
                    Update("BoardName", value);
                }
                _boardName = value;
            }
        }


        public DTask(int taskId, DateTime creationTime, string title, string description,  DateTime dueDate, string assignee, string boardCreator, string boardName) : base(new DTaskController(),boardCreator+boardName+taskId)
        {
            this.taskId = taskId;
            Title = title;
            Description = description;
            this.creationTime = creationTime;
            DueDate = dueDate;
            Assignee = assignee;
            BoardCreator = boardCreator;
            BoardName = boardName;
        }
    }
}
