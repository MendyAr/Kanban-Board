namespace IntroSE.Kanban.Backend.DataLayer
{
    internal abstract class DTO
    {
        public string ID { get; set; }
        public bool Persist { get; set; }
        
        protected DalController _controller;
        protected DTO(DalController controller)
        {
            _controller = controller;
            Persist= false;
        }

        public abstract void insert(); // insert a new line to matching table

        public void update()
        {

        }

        protected abstract string buildUpdateSqlSyntax(string[] keys, string attributeName, string attributeValue);

    }
}
