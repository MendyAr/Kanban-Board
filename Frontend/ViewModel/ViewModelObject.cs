using IntroSE.Kanban.Frontend.Model;

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
