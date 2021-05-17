using System;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DTask : DTO
    {
        private readonly int taskId;
        private string title;
        private string description;
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private string assignee;
        private int _ordinal;
        private string _boardCreator;
        private string _boardName;

        public int TaskId
        { get { return taskId; } }
        
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
      
        public DateTime CreationTime { get { return creationTime; } }
      
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
        
        public string Assignee { get { return assignee; } 
            set
            {
                if (Persist)
                {
                    Update("Assignee", value);
                }
                assignee = value;
            } }
     
        public int Ordinal
        {
            get => _ordinal; set
            {
                if (Persist)
                {
                    Update("Ordinal", value);
                }
                _ordinal = value;
            }
        }
      
        public string BoardCreator { get => _boardCreator; set
            {
                if (Persist)
                {
                    Update("BoardCreator", value);
                }
                _boardCreator = value;
            } }
        
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

        public DTask(int taskId, DateTime creationTime, string title, string description,  DateTime dueDate, string assignee, int ordinal, string boardCreator, string boardName) : base(new DTaskController(),boardCreator+boardName+taskId)
        {
            this.taskId = taskId;
            Title = title;
            Description = description;
            this.creationTime = creationTime;
            DueDate = dueDate;
            Assignee = assignee;
            Ordinal = ordinal;
            BoardCreator = boardCreator;
            BoardName = boardName;
        }
    }
}
