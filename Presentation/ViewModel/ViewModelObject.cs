using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class ViewModelObject :NotifiableObject
    {
        protected BackendController backendController;

        public ViewModelObject()
        {
            this.backendController = new BackendController();
        }
    }
}
