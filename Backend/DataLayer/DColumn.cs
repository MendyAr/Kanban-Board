namespace IntroSE.Kanban.Backend.DataLayer
{
    class DColumn : DTO
    {
        private string _creator;
        public string Creator { get => _creator; set
            {
                if (Persist)
                {
                    Update("Creator", value);
                }
                _creator = value;
            } }

        private string _board;
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


        private readonly int _ordinal;
        public int Ordinal { get => _ordinal; }
        public DColumn(string creator, string board,int ordinal) :base (new DColumnController(), creator + board + ordinal)
        {
            _creator = creator;
            _board= board;
            _ordinal = ordinal;
        }
    }
}
