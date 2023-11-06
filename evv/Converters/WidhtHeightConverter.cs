using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Evva.Converters
{
    class WidhtHeightConverter : IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            {
            try
            { 
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            if (parameter != null)
            {
                string param = parameter.ToString();
                double paramD = double.Parse(param, provider);
                double returned = System.Convert.ToDouble(value) / paramD;
                return returned;
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
            return 1;
            }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DependencyProperty.UnsetValue;
            }
    }

    class FontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            { 
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            if (parameter != null)
            {
                string param = parameter.ToString();
                double paramD = double.Parse(param, provider);
                double returned = System.Convert.ToDouble(value) / paramD;
                if (returned <= 1)
                {
                    return 1;
                }
                return returned;
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    class TwoPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            { 
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            if (parameter != null)
            {
                string param = parameter.ToString();
                double paramD = double.Parse(param, provider);
                string returned = ((int)(System.Convert.ToDouble(value) / paramD)).ToString();
                returned += ",6";
                return returned;
            }
            return ((int)value + ",6").ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
                return "0,0";
            }

}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            { 
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            if (parameter != null)
            {
                string param = parameter.ToString();
                double paramD = double.Parse(param, provider);
                string returned = ((int)(System.Convert.ToDouble(value) / paramD)).ToString();
                returned += ",";
                returned += ((int)(System.Convert.ToDouble(value) / paramD)).ToString();
                return returned;
            }
            return ((int)value + "," + value).ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
                return "0,0";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return DependencyProperty.UnsetValue;
        }
    }
}
