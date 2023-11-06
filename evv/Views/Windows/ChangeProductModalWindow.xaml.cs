using Evva.Models;
using MahApps.Metro.Controls;
using Evva.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Evva.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeProductModalWindow.xaml
    /// </summary>
    public partial class ChangeProductModalWindow : MetroWindow
    {
        public ChangeProductModalWindow()
        {
            InitializeComponent();
        }

        public ChangeProductModalWindow(Report LastSelected)
        {
            InitializeComponent();
            DataContext = new ChangeProductViewModel(LastSelected);
        }

    }
}
