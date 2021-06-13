using IntroSE.Kanban.Frontend.ViewModel;
using Presentation.Model;
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
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        private UserViewModel viewModel;
        public UserView(User user )
        {
            InitializeComponent();
            this.viewModel = new UserViewModel(user);
            DataContext = viewModel;

        }

        private void Select_Board_Click(object sender, RoutedEventArgs e)
        {
            Board board = viewModel.OpenBoard();
            if (board != null)
            {
                BoardView boardWin = new BoardView(board);
                boardWin.Show();
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login loginWin = new Login();
            loginWin.Show();
            this.Close();
        }

        private void Join_Board_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Join_Board();
        }

        private void Create_Board_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CreateBoard();
        }

        private void In_Progress_Tasks_Click()
        {

        }
    }
}