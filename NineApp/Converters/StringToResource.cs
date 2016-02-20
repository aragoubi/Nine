using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the Resource corresponding to the given Binding value
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl Content="{Binding STRING_PROPERTY, Converter={StaticResource StringToResource}"/>
    /// </code>
    /// </example>
    internal class StringToResource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceKey = "tag_" + value;
            var resource = Application.Current.TryFindResource(resourceKey);
            if (resource == null)
            {
                throw new ResourceReferenceKeyNotFoundException(
                    "The resource \"" + resourceKey + "\" does not exist !", resourceKey);
            }
            return resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("StringToResource is a One-Way converter only !");
        }
    }
}