//using Evva.Context.UnitOfWork;
//using Evva.Commands;
//using Evva.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Evva.Hash;
//using System.Windows;
//using System.Text.RegularExpressions;

//namespace Evva.ViewModels
//{
//    public class RegisterPageViewModelTwo : BaseViewModel
//    {
//        private string userLogin;
//        private string message;
//        private string userPassword;

//        private string userName;
//        private string userSurname;
//        private string userLastname;


//        private UsersParam usersParam;
//        private UsersDatum usersDatum;


//        public RegisterPageViewModelTwo()
//        {
//            try
//            {
//                UpdateViewCommand = new UpdateViewCommand(this);
//                Message = "Придумайте логин(от 5 до 20 символов) и пароль(от 8 до 20 символов). Логин и пароль могут содержать латинские символы и цифры";
//                usersDatum = new UsersDatum();
//                usersParam = new UsersParam();
//                usersParam.ParamsDate = DateTime.Now;
//                usersDatum.Birthday = DateTime.Now;

//                UserLastname = "";
//                UserSurname = "";
//                UserName = "";

//                UserWeight = 10;
//                UserHeight = 50;
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
//            }
//        }


//        #region Properties

//        public decimal UserWeight
//        {
//            get { return usersParam.UserWeight; }
//            set
//            {
//                usersParam.UserWeight = value;
//                OnPropertyChanged("UserWeight");
//            }
//        }
//        public int UserHeight
//        {
//            get { return usersParam.UserHeight; }
//            set
//            {
//                usersParam.UserHeight = value;
//                OnPropertyChanged("UserHeight");
//            }
//        }
//        public string UserLogin
//        {
//            get { return userLogin; }
//            set
//            {
//                userLogin = value;
//                OnPropertyChanged("UserLogin");
//            }
//        }
//        public string UserPassword
//        {
//            get { return userPassword; }
//            set
//            {
//                userPassword = value;
//                OnPropertyChanged("UserPassword");
//            }
//        }
//        public string Message
//        {
//            get { return message; }
//            set
//            {
//                message = value;
//                OnPropertyChanged("Message");
//            }
//        }
//        public string UserName
//        {
//            get { return userName; }
//            set
//            {
//                userName = value;
//                OnPropertyChanged("UserName");
//            }
//        }
//        public string UserSurname
//        {
//            get { return userSurname; }
//            set
//            {
//                userSurname = value;
//                OnPropertyChanged("UserSurname");
//            }
//        }
//        public string UserLastname
//        {
//            get { return userLastname; }
//            set
//            {
//                userLastname = value;
//                OnPropertyChanged("UserLastname");
//            }
//        }
//        public DateTime DateToChoose
//        {
//            get { return usersDatum.Birthday; }
//            set
//            {
//                usersDatum.Birthday = value;
//                OnPropertyChanged("DateToChoose");
//            }
//        }

//        #endregion 

//        #region Commands

//        #region Перейти на окно входа

//        public ICommand UpdateViewCommand { get; set; }


//        #endregion

//        #region Выполнить регистрацию

//        private DelegateCommand registerCommand;

//        public ICommand RegisterCommand
//        {
//            get
//            {
//                if (registerCommand == null)
//                {
//                    registerCommand = new DelegateCommand(Register, CanRegister);
//                }
//                return registerCommand;
//            }
//        }

//        private void Register()
//        {
//            try
//            {
//                using (UnitOfWork unit = new UnitOfWork())
//                {
//                    IEnumerable<User> result = unit.UserRepository.Get(x => x.UserLogin == UserLogin);
//                    if (result.Count() != 0)
//                    {
//                        Message = "Пользователь с таким логином уже существует!";
//                        return;
//                    }
//                    else
//                    {
//                        User regUser = new User();
//                        regUser.Salt = PasswordHash.GenerateSaltForPassword().ToString();
//                        regUser.UserPassword = PasswordHash.ComputePasswordHash(UserPassword, int.Parse(regUser.Salt));
//                        regUser.UserLogin = UserLogin;

//                        unit.UserRepository.Create(regUser);
//                        unit.Save();

//                        User foundUser = unit.UserRepository.Get(x => x.UserLogin == regUser.UserLogin).First();

//                        usersDatum.IdData = foundUser.Id;
//                        usersDatum.FullName = UserSurname + " " + UserName + " " + UserLastname;

//                        unit.UserDatumRepository.Create(usersDatum);
//                        unit.Save();

//                        usersParam.IdParams = foundUser.Id;

//                        unit.UserParamRepository.Create(usersParam);
//                        unit.Save();

//                        Message = "Регистрация прошла успешно! Введите логин и пароль";
//                        UpdateViewCommand.Execute("LogInPage");
//                    }
//                }
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
//            }
//        }

//        private bool CanRegister()
//        {
//            try
//            {
//                if (UserLogin == "" || UserPassword == "" || UserWeight == 0 || UserHeight == 0 || UserName == "" || UserLastname == "" || UserSurname == "")
//                {
//                    return false;
//                }
//                else if (DateToChoose.CompareTo(DateTime.Today.Date) >= 0)
//                {
//                    return false;
//                }
//                else if (!Regex.IsMatch(UserLogin, "^([a-z]|[A-Z]|[0-9]){5,20}$") ||
//                    !Regex.IsMatch(UserPassword, "^([a-z]|[A-Z]|[0-9]){8,20}$") ||
//                    !Regex.IsMatch(UserName, "^[А-Я]{1}[а-я]{1,99}$") ||
//                    !Regex.IsMatch(UserLastname, "^[А-Я]{1}[а-я]{1,99}$") ||
//                    !Regex.IsMatch(UserSurname, "^[А-Я]{1}[а-я]{1,99}$"))
//                {
//                    return false;
//                }
//                else
//                {
//                    return true;
//                }
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
//                return false;
//            }
//        }

//        #endregion 


//        #endregion

//    }
//}
