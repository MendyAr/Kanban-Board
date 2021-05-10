namespace IntroSE.Kanban.Backend.DataLayer
{
    internal abstract class DTO
    {
        protected string _id;
        public string ID { get => _id; set {
                if (Persist)
                {
                    _controller.Update(_id, "ID", value);
                }
                _id = value;
            } }
        public bool Persist { get; set; }
        
        protected DalController _controller;
        protected DTO(DalController controller, string id)
        {
            _controller = controller;
            Persist= false;
            ID = id;
        }

        protected void insert()
        {
            _controller.insert(this);
        }

        protected abstract string buildUpdateSqlSyntax(string[] keys, string attributeName, string attributeValue);

    }
}
