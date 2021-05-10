

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoard : DTO
    {
        
        private readonly string _creator;
        public string Creator { get => _creator;}


        private string _boardName;
        public string BoardName { get => _boardName; set 
            {

                if (Persist)
                {
                    Update("Name", value);
                }
                _boardName = value;
            } }


        private int _limitColumn1;
        public int LimitColumn1
        {
            get => _limitColumn1; set
            {
                if (Persist)
                {
                    Update("LimitColumn1", value);
                }
                _limitColumn1 = value;
            }
        }


        private int _limitColumn2;
        public int LimitColumn2
        {
            get => _limitColumn2; set
            {
                if (Persist)
                {
                    Update("LimitColumn2", value);
                }
                _limitColumn2 = value;
            }
        }


        private int _limitColumn3;
        public int LimitColumn3
        {
            get => _limitColumn3; set
            {
                if (Persist)
                {
                    Update("LimitColumn3", value);
                }
                _limitColumn3 = value;
            }
        }

        private DColumn[] columns;
        public DColumn[] Columns { get => columns; }

        public DBoard (string creator, string boardName,int limitColumn1, int limitColumn2, int limitColumn3) : base(new DBoardController(),creator + boardName)
        {
            this._creator = creator;
            this._boardName = boardName;
            _limitColumn1 = limitColumn1;
            _limitColumn2 = limitColumn2;
            _limitColumn3 = limitColumn3;
            columns = new DColumn[3];
        }
    }
}
