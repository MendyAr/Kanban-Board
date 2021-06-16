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
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Window
    {

        private BoardViewModel ViewModel;
        public Board(BoardModel board)
        {
            InitializeComponent();
            this.ViewModel = new BoardViewModel(board);
            this.DataContext = ViewModel;
        }

        private void Add_Column(object sender, RoutedEventArgs e)
        {
            ViewModel.AddColumn();
        }

        private void Add_Task(object sender, RoutedEventArgs e)
        {
            AddingNewTask newTask = new AddingNewTask(ViewModel.Board);
            newTask.Show();
        }

        private void Delete_Column(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteColumn();
        }

        private void Show_Column(object sender, RoutedEventArgs e)
        {
            ColumnModel column = ViewModel.GetColumn();
            if(column != null)
            {
                ColumnView columnView = new ColumnView(column);
                columnView.Show();
                this.Close();
            }
        }

        private void Roll_Back(object sender, RoutedEventArgs e)
        {
            UserView userView = new UserView(ViewModel.Board.User);
            userView.Show();
            this.Close();
        }

    }
}
