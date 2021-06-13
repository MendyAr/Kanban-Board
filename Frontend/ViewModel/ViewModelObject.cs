using IntroSE.Kanban.Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public abstract class ViewModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        public ViewModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
