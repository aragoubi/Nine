using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Visibility
    /// Visibility.Visible if Binding value #1 is equal to the Binding value #2
    /// or
    /// Visibility.Collapsed if Binding value #1 is not equal to the Binding value #2
    /// (Usefull to show a specific ITEM inside an <ItemsControl> of ITEMS and hide the others)
    /// </summary>
    /// <example>
    /// How to use
    /// (You can reverse the returned Visibility by giving "ReverseVisibility" as ConverterParameter)
    /// <code>
    /// <UserControl>
    ///     <UserControl.IsEnabled>
    ///         <MultiBinding Converter="{StaticResource ItemsEqualToVisibility}">
    ///             <Binding Path="ITEM1_PROPERTY"/>
    ///             <Binding Path="ITEM2_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class ItemsEqualToVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2)
            {
                throw new ArgumentException("ItemsEqualToVisibility converter needs 2 values [item1, item2] !", "values");
            }

            var equal = Visibility.Visible;
            var notEqual = Visibility.Collapsed;
            if ((string) parameter == "ReverseVisibility")
            {
                var tmp = equal;
                equal = notEqual;
                notEqual = tmp;
            }

            if (values[0].Equals(values[1]))
            {
                return equal;
            }
            return notEqual;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ItemsEqualToVisibility is a One-Way converter only !");
        }
    }
}