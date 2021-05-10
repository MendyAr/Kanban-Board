using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DColumn : DTO
    {
        private string _creator;
        public string Creator { get => _creator; set
            {
                if (Persist)
                {
                    _controller.Update()
                }
            } }
        private string _board;
        private int _ordinal;
        public DColumn(string creator, string board,string ordinal) :base (new DColumnController, creator + board + ordinal)
        {

        }
    }
}
