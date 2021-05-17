using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DColumn : DTO
    {
        private string _creatorEmail;
        private string _boardName;
        private readonly int _ordinal;
        private int _limit;
        private List<DTask> _tasks;

        public string CreatorEmail { get => _creatorEmail; set
            {
                if (Persist)
                {
                    Update("Creator", value);
                }
                _creatorEmail = value;
            } }

        public string BoardName
        {
            get => _boardName; set
            {
                if (Persist)
                {
                    Update("Board", value);
                }
                _boardName = value;
            }
        }

        public int Limit { get => _limit; set
            {
                if (Persist)
                {
                    Update("Limit",value);
                }
                _limit = value;
            } }
        public int Ordinal { get => _ordinal; }

        public List<DTask> Tasks { get => _tasks; set {
                _tasks = value;
            } }

        public DColumn(string creatorEmail, string boardName,int ordinal, int limit) :base (new DColumnController(), creatorEmail + boardName + ordinal)
        {
            _creatorEmail = creatorEmail;
            _boardName= boardName;
            _ordinal = ordinal;
            _limit = limit;
        }
    }
}
