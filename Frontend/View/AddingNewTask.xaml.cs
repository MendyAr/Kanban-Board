using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
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

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for AddingNewTask.xaml
    /// </summary>
    public partial class AddingNewTask : Window
    {
        private AddingNewTaskViewModel _viewModel;
        public AddingNewTask(BoardModel board)
        
        {
            InitializeComponent();
            _viewModel = new AddingNewTaskViewModel(board);
            DataContext = _viewModel;

        }

        private void Add_Task_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddTask();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
