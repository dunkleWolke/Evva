using Evva.Context.UnitOfWork;
using Evva.DeserializedUserNamespace;
using Evva.Models;
using Evva.Options;
using Evva.ViewModels;
using Evva.Views;
using Evva.Views.Windows;
using Evva.XMLSerializer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Evva
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            List<OptionsPack> optionsPacks = XmlSerializeWrapper<List<OptionsPack>>.Deserialize("../appSettings.xml", FileMode.Open);
            OptionsPack currentUserSettings = optionsPacks.Find(x => x.OptionUserId == DeserializedUser.deserializedUser.Id);
            //if (currentUserSettings?.IsSplashScreenShown ?? true)
            //{
            //    SplashScreen splash = new SplashScreen("../Resources/EvvaSplash.png");
            //    splash.Show(autoClose: false, topMost: false);
            //    splash.Close(TimeSpan.FromSeconds(1));
            //}
            using (UnitOfWork unit = new UnitOfWork())
            {
                IEnumerable<User> resultUserFound = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin);

                if (resultUserFound.Count() != 0 && (currentUserSettings?.IsStayAuthorized ?? false))
                {
                    if (resultUserFound.First<User>().UserPassword.SequenceEqual<byte>(DeserializedUser.deserializedUser.UserPassword))
                    {
                        MainWindow mainWindow = new MainWindow();

                        mainWindow.Show();

                        currentUserSettings.setAppTheme();
                        currentUserSettings.setAppAccent();

                        OptionsViewModel.OptionsPack = currentUserSettings;
                    }
                    else
                    {
                        LogInWindow logInWindow = new LogInWindow();
                        logInWindow.Show();
                    }
                }
                else
                {
                    LogInWindow logInWindow = new LogInWindow();
                    logInWindow.Show();
                }
            }

        }
    }
}
