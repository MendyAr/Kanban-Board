using Frontend.ViewModel;
using Presentation.Model;
using System.Windows;
using System.Windows.Controls;


namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginViewModel viewModel;
        public Login()
        {
            InitializeComponent();
            this.viewModel = new LoginViewModel();
            this.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User user = viewModel.Login();
            if(user != null)
            {
                BoardView boardView = new BoardView(user);
                boardView.Show();
                this.Close();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
