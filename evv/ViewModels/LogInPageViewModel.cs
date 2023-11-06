using Evva.Context.UnitOfWork;
using Evva.Commands;
using Evva.Models;
using Evva.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Evva.Hash;
using Evva.XMLSerializer;
using System.IO;
using Evva.Options;
using Evva.DeserializedUserNamespace;

namespace Evva.ViewModels
{
    public class LogInPageViewModel : BaseViewModel
    {
        private User User = new User();
        private string userPassword;
        private string message;

        #region Properties

        public string UserLogin
        {
            get { return User.UserLogin; }
            set
            {
                User.UserLogin = value;
                OnPropertyChanged("UserLogin");
            }
        }
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }
        public bool UserIsAdmin
        {
            get { return (bool)User.IsAdmin; }
            set
            {
                User.IsAdmin = value;
                OnPropertyChanged("UserIsAdmin");
            }
        }
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }
        #endregion 

        #region Commands

        #region Перейти на окно регистрации

        public ICommand UpdateViewCommand { get; set; }

        public LogInPageViewModel()
        {
            try
            { 
            UpdateViewCommand = new UpdateViewCommand(this);
            Message = "Welcome back! \n Enter your login and password!";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion

        #region Выполнить вход

        private DelegateCommand logInCommand;

        public ICommand LogInCommand
        {
            get
            {
                if (logInCommand == null)
                {
                    logInCommand = new DelegateCommand(LogIn);
                }
                return logInCommand;
            }
        }

        private void LogIn()
        {
            try
            { 
            using(UnitOfWork uow = new UnitOfWork())
            {
                List<User> result = uow.UserRepository.Get(x => x.UserLogin == User.UserLogin).ToList();
                
                if (result.Count() == 0)
                {
                    Message = "Логин или пароль неверный!";
                    return;
                }
                else if(PasswordHash.IsPasswordValid(UserPassword, int.Parse(result.First<User>().Salt), result.First<User>().UserPassword))
                {
                    User deserializedeUser = result.First();
                    XmlSerializeWrapper<User>.Serialize(deserializedeUser, "../lastUser.xml");

                    DeserializedUser.deserializedUser = XmlSerializeWrapper<User>.Deserialize("../lastUser.xml", FileMode.OpenOrCreate);

                    List<OptionsPack> optionsPacks = XmlSerializeWrapper<List<OptionsPack>>.Deserialize("../appSettings.xml", FileMode.OpenOrCreate);
                    OptionsViewModel.OptionsPack = optionsPacks.Find(x => x.OptionUserId == deserializedeUser.Id);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    OptionsViewModel.OptionsPack?.setAppAccent();
                    OptionsViewModel.OptionsPack?.setAppTheme();

                    OptionsPack optionsPackUser = optionsPacks.Find(x => x.OptionUserId == DeserializedUser.deserializedUser.Id);

                    if (optionsPackUser == null)
                    {
                        optionsPacks.Add(new OptionsPack());
                        XmlSerializeWrapper<List<OptionsPack>>.Serialize(optionsPacks, "../appSettings.xml");
                    }

                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is LogInWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                }
                else
                {
                    Message = "Логин или пароль неверный!";
                    return;
                }             
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion 


        #endregion

    }
}
