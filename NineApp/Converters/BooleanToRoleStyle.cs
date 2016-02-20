using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives you the corresponding Style
    /// KEY_OF_THE_STYLE+"TeacherStyle" if the Binding value is True
    /// or
    /// KEY_OF_THE_STYLE+"StudentStyle" if the Binding value is False
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl Style="{Binding IS_TEACHER_PROPERTY, Converter={StaticResource BooleanToRoleStyle}, ConverterParameter=KEY_OF_THE_STYLE}"/>
    /// </code>
    /// </example>
    internal class BooleanToRoleStyle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter", "BooleanToRoleStyle converter needs the parameter !");
            }

            var query = "";
            var isTeacher = (bool) value;
            var param = (string) parameter;
            if (isTeacher)
            {
                query = "" + param + "TeacherStyle";
            }
            else
            {
                query = "" + param + "StudentStyle";
            }
            var resource = Application.Current.TryFindResource(query);
            if (resource == null)
            {
                throw new ResourceReferenceKeyNotFoundException("The resource \"" + query + "\" haven't been found",
                    query);
            }
            return resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("BooleanToVisibility is a One-Way converter only !");
        }
    }
}