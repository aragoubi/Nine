using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Boolean
    /// True if Binding value CURRENT_WINDOWS_STATE is equal to the Parameter value TARGET_WINDOW_STATE
    /// or
    /// False if Binding value CURRENT_WINDOWS_STATE is not equal to the Parameter value TARGET_WINDOW_STATE
    /// (Usefull to enable specific Controls when the Window is Maximized)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl IsEnabled="{Binding CURRENT_WINDOW_STATE, Converter={StaticResource WindowStateToBoolean}, ConverterParameter=TARGET_WINDOW_STATE}"/>
    /// </code>
    /// </example>
    internal class WindowStateToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter", "WindowStateToBoolean converter needs the parameter !");
            }

            WindowState state;
            switch ((string) parameter)
            {
                case "Maximized":
                    state = WindowState.Maximized;
                    break;
                case "Minimized":
                    state = WindowState.Minimized;
                    break;
                case "Normal":
                    state = WindowState.Normal;
                    break;
                default:
                    throw new ArgumentNullException("parameter",
                        "Invalid parameter, must be \"Maximized\", \"Minimized\" or \"Normal\" !");
            }

            var equal = true;
            var notEqual = false;
            if (parameter == null)
            {
                var tmp = equal;
                equal = notEqual;
                notEqual = tmp;
            }

            if ((WindowState) value == state)
            {
                return equal;
            }
            return notEqual;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("WindowStateToBoolean is a One-Way converter only !");
        }
    }
}