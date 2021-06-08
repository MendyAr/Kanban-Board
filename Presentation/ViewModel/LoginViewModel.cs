using Presentation.Model;
using System;

namespace Frontend.ViewModel
{
    class LoginViewModel : ViewModelObject
    {
        private string _username;
        private string _password;
        private string _message;

        public string Username 
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }
        public User Login()
        {
            Message = "";
            try
            {
                return backendController.Login(Username, Password); 
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
