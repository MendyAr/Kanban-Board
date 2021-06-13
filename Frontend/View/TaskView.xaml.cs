using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskViewModel = IntroSE.Kanban.Frontend.ViewModel.TaskViewModel;

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        TaskViewModel taskVM;
        public TaskView()
        {
            InitializeComponent();
            taskVM = new TaskViewModel();
            DataContext = taskVM;
        }
    }
}
