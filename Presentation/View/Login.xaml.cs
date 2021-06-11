using Frontend.ViewModel;
using Presentation.Model;
using System.Windows;


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

        

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User user = viewModel.Login();
            if(user != null)
            {
                UserView boardView = new UserView(user);
                boardView.Show();
                this.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
             viewModel.Register();
        }
    }
}
