using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Spacebardesktop.Models;
using Spacebardesktop.Repositories;
using Spacebardesktop.View;

namespace Spacebardesktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        //fields 
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;


        private IUserRepository userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { _currentUserAccount = value;  OnPropertyChanged(nameof(CurrentUserAccount)); }
        
        }

        public ViewModelBase CurrentChildView 
        {
            get{return _currentChildView;}
            set{ _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }

        }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; OnPropertyChanged(nameof(Caption)); }
        }

        //comandos
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }

        public MainViewModel()
        {
           userRepository = new UserRepository();
           CurrentUserAccount = new UserAccountModel();
           

            //inicialização dos comandos

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteCustomerViewCommand);

            //Default View
            ExecuteShowHomeViewCommand(null);
            LoadCurrentUserData();
        }

        private void ExecuteCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomViewModel();
            Caption = "Customer";
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"{user.Name}";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
               CurrentUserAccount.DisplayName="Usuario Invalido";
              // Hide child views
            }
        }
    }
}
 