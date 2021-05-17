using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DColumn : DTO
    {
        private string _creator;
        private string _board;
        private readonly int _ordinal;
        private int _limit;
        private List<DTask> _tasks;

        public string Creator { get => _creator; set
            {
                if (Persist)
                {
                    Update("Creator", value);
                }
                _creator = value;
            } }

        public string Board
        {
            get => _board; set
            {
                if (Persist)
                {
                    Update("Board", value);
                }
                _board = value;
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

        public DColumn(string creator, string board,int ordinal, int limit) :base (new DColumnController(), creator + board + ordinal)
        {
            _creator = creator;
            _board= board;
            _ordinal = ordinal;
            _limit = limit;
        }
    }
}
