using ControlzEx.Theming;
using Evva.DeserializedUserNamespace;
using Evva.Models;
using Evva.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Evva.Options
{
    public class OptionsPack
    {
        public OptionsPack()
        {
            IsSplashScreenShown = true;
            IsStayAuthorized = true;

            CurrentAppAccent = "LightGray";
            CurrentAppTheme = "Violet";

            OptionUserId = DeserializedUser.deserializedUser.Id;
        }

        public int OptionUserId { get; set; }

        public bool IsSplashScreenShown { get; set; }
        public bool IsStayAuthorized { get; set; }

        public string CurrentAppTheme { get; set; }
        public string CurrentAppAccent { get; set; }

        #region Открытые методы

        public  void setAppTheme()
        {
            ThemeManager.Current.ChangeThemeBaseColor(Application.Current, this.CurrentAppTheme);
            Application.Current?.MainWindow?.Activate();
        }

        public void setAppAccent()
        {
            ThemeManager.Current.ChangeThemeColorScheme(Application.Current, this.CurrentAppAccent);
            Application.Current?.MainWindow?.Activate();
        }


        #endregion
    }
}
