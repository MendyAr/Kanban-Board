

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

        private DColumn[] columns;
        public DColumn[] Columns { get => columns; }

        public DBoard (string creator, string boardName) : base(new DBoardController(),creator + boardName)
        {
            this._creator = creator;
            this._boardName = boardName;
            columns = new DColumn[3];
        }
    }
}
