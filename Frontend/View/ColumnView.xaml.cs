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
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : Window
    {

        private ColumnViewModel viewModel;
        public ColumnView(ColumnModel columnModel)
        {
            InitializeComponent();
            this.viewModel = new ColumnViewModel(columnModel);
            this.DataContext = viewModel;
        }

        private void Roll_Back(object sender, RoutedEventArgs e)
        {
            Board boardView = new Board(viewModel.Column.Board);
            boardView.Show();
            this.Close();
        }

        private void Button_Click_Advance_Task(object sender, RoutedEventArgs e)
        {
            viewModel.AdvanceTask();
        }

        private void Button_Click_Show_Task(object sender, RoutedEventArgs e)
        {
            TaskView taskView = new TaskView(viewModel.getSelectedTask());
            if (taskView != null)
            {
                taskView.Show();
            }
        }

    }
}
