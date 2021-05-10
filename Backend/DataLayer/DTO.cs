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

        public void Insert()
        {
            _controller.Insert(this);
        }

        protected void Update(string attributeName, string attributeValue)
        {
            _controller.Update(ID, attributeName, attributeValue);
        }

        protected void Update(string attributeName, long attributeValue)
        {
            _controller.Update(ID, attributeName, attributeValue);
        }


    }
}
