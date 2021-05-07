using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal abstract class DTO
    {
        public string ID { get; set; }
        public bool persist;
        protected DalController _controller;
        protected DTO(DalController controller)
        {
            _controller = controller;
            persist = false;
        }

        public abstract void insert(); // insert a new line to matching table

    }
}
