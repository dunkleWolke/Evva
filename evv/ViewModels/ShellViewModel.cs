using Evva.Context.UnitOfWork;
using Evva.DeserializedUserNamespace;
using Evva.Models;
using Evva.Views.Pages;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Evva.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        private static readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private static readonly ObservableCollection<MenuItem> AppOptionsMenu = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Menu => AppMenu;

        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;

        public ShellViewModel()
        {
            try
            { 
            Menu.Clear();
            OptionsMenu.Clear();
            // Строим меню
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ChartPieSolid },
                Label = "Ежедневная статистика",
                NavigationType = typeof(TodayResultsView),
                NavigationDestination = new Uri("Views/Pages/TodayResultsView.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.LemonRegular },
                Label = "Приемы пищи",
                NavigationType = typeof(TodayDietView),              
                NavigationDestination = new Uri("Views/Pages/TodayDietView.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CalendarPlusRegular },
                Label = "Добавить продукт",
                NavigationType = typeof(AddProductView),
                NavigationDestination = new Uri("Views/Pages/AddProductView.xaml", UriKind.RelativeOrAbsolute)
            });
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ChartBarRegular },
                Label = "Отслеживание веса",
                NavigationType = typeof(StatisticView),
                NavigationDestination = new Uri("Views/Pages/StatisticView.xaml", UriKind.RelativeOrAbsolute)
            });
            //this.Menu.Add(new MenuItem()
            //{
            //    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Human },
            //    Label = "Параметры тела",
            //    NavigationType = typeof(ParamsView),
            //    NavigationDestination = new Uri("Views/Pages/ParamsView.xaml", UriKind.RelativeOrAbsolute)
            //});
            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.PowerOffSolid },
                Label = "Настройки",
                NavigationType = typeof(OptionsView),
                NavigationDestination = new Uri("Views/Pages/OptionsView.xaml", UriKind.RelativeOrAbsolute)
            });

            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid },
                Label = "О приложении",
                NavigationType = typeof(AboutAppView),
                NavigationDestination = new Uri("Views/Pages/AboutAppView.xaml", UriKind.RelativeOrAbsolute)
            });
            
            bool? isAdmin = false;

            using (UnitOfWork unit = new())
            {
                if (DeserializedUser.deserializedUser.Id != 0)
                {   if (unit.UserRepository.FindById(DeserializedUser.deserializedUser.Id) != null)
                    {
                        isAdmin = unit.UserRepository.FindById(DeserializedUser.deserializedUser.Id).IsAdmin;
                    }
                    else
                    {
                        MessageBox.Show("Приложение завершило свою работу в связи с неполадками при подключении к базе данных. Попробуйте запустить приложение снова.","Ошибка");
                    }
                }
            }

            if ((bool)isAdmin)
                {
                this.OptionsMenu.Add(new MenuItem()
                {
                    Icon = new PackIconModern() { Kind = PackIconModernKind.ControlGuide },
                    Label = "Администрирование",
                    NavigationType = typeof(AdminView),
                    NavigationDestination = new Uri("Views/Pages/AdminView.xaml", UriKind.RelativeOrAbsolute)
                });
                };
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

    }
}
