using ControlzEx.Theming;
using Evva.Commands;
using Evva.Context.UnitOfWork;
using Evva.DeserializedUserNamespace;
using Evva.Hash;
using Evva.Models;
using Evva.Options;
using Evva.Views.Windows;
using Evva.XMLSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Evva.ViewModels
{
    class OptionsViewModel : BaseViewModel
    {
        private string textMessage;
        public string[] messages = {
            "Взрослому человеку нужно примерно 8 стаканов жидкости в день." +
            " В это количество входят все напитки и жидкие продукты (например, супы)." +
            " Но лучше заменить крепкий чай и кофе на чистую воду. ",

            "Сытный сбалансированный приём пищи с утра обеспечит вас бодростью и силами до обеда.",

            "Постарайтесь перед сном выдержать хотя бы два часа без еды. " +
            "Распределите приёмы пищи по дню так, чтобы не было перерыва больше 3-4 часов.",

            "Взрослому человеку, чтобы быть здоровым, нужно спать минимум 7-8 часов в сутки." +
            " Мы можем некоторое время работать на износ и бодрствовать по 20 часов, но в итоге это приведёт к проблемам и плохому самочувствию.",

            "Если у вас явный недостаток или избыток массы — постарайтесь это исправить." +
            " И то, и другое отрицательно влияет на здоровье опорной и сердечно-сосудистой систем.",

            "Тщательно мойте руки перед едой, после туалета или улицы." +
            " Душ следует принимать ежедневно, а чистить зубы — минимум дважды в день.",

            "Нет смысла пытаться сразу обзавестись всеми этими привычками." +
                " Лучше начинать постепенно, чтобы они не вызывали напряжения и прочно вошли в вашу жизнь.",

            "Регулярно проходите медосмотры. Так можно вовремя заметить заболевания, когда их проще всего вылечить." +
            " Лучше всего делать это хотя бы раз в год.",

            "Жесткие диеты не несут в себе пользу для здоровья!"};
        private string oldPassword;
        private string newPassword;


        public OptionsViewModel()
        {
            try
            { 
            if (OptionsPack == null)
            {
                OptionsPack = new();
            }
            Random random = new();
            TextMessage = messages[random.Next(0, messages.Length)];
            NewPassword = "";
            OldPassword = "";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #region Properties

        public static OptionsPack OptionsPack { get; set; }

        public string OldPassword
        {
            get { return oldPassword; }
            set
            {
                oldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }

        public string TextMessage
        {
            get { return textMessage; }
            set
            {
                textMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }
        public bool IsShowSplash
        {
            get { return OptionsPack.IsSplashScreenShown; }
            set
            {
                OptionsPack.IsSplashScreenShown = value;
                OnPropertyChanged("IsShowSplash");
            }
        }
        public bool IsStayAuthorized
        {
            get { return OptionsPack.IsStayAuthorized; }
            set
            {
                OptionsPack.IsStayAuthorized = value;
                OnPropertyChanged("IsStayAuthorized");
            }
        }


        #endregion

        #region Commands
        #region Изменение темы

        private DelegateCommand<object> changeAppThemeCommand;

        public ICommand ChangeAppThemeCommand
        {
            get
            {
                if (changeAppThemeCommand == null)
                {
                    changeAppThemeCommand = new DelegateCommand<object>(changeAppTheme);
                }
                return changeAppThemeCommand;
            }
        }

        private void changeAppTheme(object sender)
        {
            try
            { 
            ThemeManager.Current.ChangeThemeBaseColor(Application.Current, sender.ToString());
            Application.Current?.MainWindow?.Activate();
            OptionsPack.CurrentAppTheme = sender.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion
        #region Изменение акцента

        private DelegateCommand<object> changeAppAccentСommand;

        public ICommand ChangeAppAccentСommand
        {
            get
            {
                if (changeAppAccentСommand == null)
                {
                    changeAppAccentСommand = new DelegateCommand<object>(changeAppAccent);
                }
                return changeAppAccentСommand;
            }
        }

        private void changeAppAccent(object sender)
        {
            try
            { 
            ThemeManager.Current.ChangeThemeColorScheme(Application.Current, sender.ToString());
            Application.Current?.MainWindow?.Activate();
            OptionsPack.CurrentAppAccent = sender.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion
        #region Сохранение настроек

        private DelegateCommand saveApplicationSettingsСommand;

        public ICommand SaveApplicationSettingsСommand
        {
            get
            {
                if (saveApplicationSettingsСommand == null)
                {
                    saveApplicationSettingsСommand = new DelegateCommand(saveApplicationSettings);
                }
                return saveApplicationSettingsСommand;
            }
        }

        private void saveApplicationSettings()
        {
            try
            { 
            List<OptionsPack> optionsPacks = XmlSerializeWrapper<List<OptionsPack>>.Deserialize( "../appSettings.xml", FileMode.OpenOrCreate);
            optionsPacks.Remove(optionsPacks.Find(x => x.OptionUserId == DeserializedUser.deserializedUser.Id));
            optionsPacks.Add(OptionsPack);
            XmlSerializeWrapper<List<OptionsPack>>.Serialize(optionsPacks, "../appSettings.xml");
            TextMessage = "Настройки сохранены!";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion
        #region Установка настроек по умолчанию

        private DelegateCommand setDefaultSettingsСommand;

        public ICommand SetDefaultSettingsСommand
        {
            get
            {
                if (setDefaultSettingsСommand == null)
                {
                    setDefaultSettingsСommand = new DelegateCommand(setDefaultSettings);
                }
                return setDefaultSettingsСommand;
            }
        }

        private void setDefaultSettings()
        {
            try
            { 
            IsShowSplash = true;
            IsStayAuthorized = true;
            OptionsPack.CurrentAppTheme = "Light";
            OptionsPack.CurrentAppAccent = "ForestGreen";


            List<OptionsPack> optionsPacks = XmlSerializeWrapper<List<OptionsPack>>.Deserialize("../appSettings.xml", FileMode.OpenOrCreate);
            optionsPacks.Remove(optionsPacks.Find(x => x.OptionUserId == DeserializedUser.deserializedUser.Id));
            optionsPacks.Add(OptionsPack);
            XmlSerializeWrapper<List<OptionsPack>>.Serialize(optionsPacks, "../appSettings.xml");

            ThemeManager.Current.ChangeThemeColorScheme(Application.Current, OptionsPack.CurrentAppAccent);
            ThemeManager.Current.ChangeThemeBaseColor(Application.Current, OptionsPack.CurrentAppTheme);
            Application.Current?.MainWindow?.Activate();
            TextMessage = "Установлены настройки по умолчанию! Перезагрузите приложение для обновления цветовой палитры.";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion

        #region Изменение пароля

        private DelegateCommand changePasswordCommand;

        public ICommand ChangePasswordCommand
        {
            get
            {
                if (changePasswordCommand == null)
                {
                    changePasswordCommand = new DelegateCommand(changePassword);
                }
                return changePasswordCommand;
            }
        }

        private void changePassword()
        {
            try
            { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                IEnumerable<User> result = unit.UserRepository.Get(x => x.Id == DeserializedUser.deserializedUser.Id);
                User user = result?.First();
                if (NewPassword == "" || OldPassword == "")
                {
                    TextMessage = "Пустые поля!";
                }
                else if (PasswordHash.ComputePasswordHash(OldPassword, int.Parse(user.Salt)).SequenceEqual(user.UserPassword) && Regex.IsMatch(NewPassword, "^([a-z]|[A-Z]|[0-9]){8,20}$"))
                {
                    user.Salt = PasswordHash.GenerateSaltForPassword().ToString();
                    user.UserPassword = PasswordHash.ComputePasswordHash(NewPassword, int.Parse(user.Salt));
                    unit.UserRepository.Update(user);
                    unit.Save();
                    TextMessage = "Смена пароля прошла успешно!";
                    OldPassword = "";
                    NewPassword = "";
                }            
                else if(!PasswordHash.ComputePasswordHash(OldPassword, int.Parse(user.Salt)).SequenceEqual(user.UserPassword))
                {
                    TextMessage = "Неверный старый пароль!";
                }
                else if (!Regex.IsMatch(NewPassword, "^([a-z]|[A-Z]|[0-9]){8,20}$"))
                {
                    TextMessage = "Неверный новый пароль! Можно вводить латинские символы и цифры. Длина пароля: 8-20 символов.";
                }
                else 
                {
                    TextMessage = "Пароль не может быть изменён на самого себя!";
                }
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion

        #region Выход из аккаунта

        private DelegateCommand exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new DelegateCommand(exit);
                }
                return exitCommand;
            }
        }

        private void exit()
        {
            try
            { 
            User deserializedeUser = new();
            XmlSerializeWrapper<User>.Serialize(deserializedeUser, "../lastUser.xml");

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive && x.Name == "MainAppWindow");

            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();

            window.Close();
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



