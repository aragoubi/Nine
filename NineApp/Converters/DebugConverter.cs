using System;
using System.Globalization;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Just a Debugger that displays your Binding value before affecting it
    /// (Usefull to debug Bindings)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl Property="{Binding PROPERTY, Converter={StaticResource DebugConverter}"/>
    /// </code>
    /// </example>
    internal class DebugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine(value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("DebugConverter is a One-Way converter only !");
        }
    }
}