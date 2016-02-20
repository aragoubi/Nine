using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Boolean
    /// True if Binding value #1 is equal to the Binding value #2
    /// or
    /// False if Binding value #1 is not equal to the Binding value #2
    /// (Usefull to enable a specific ITEM inside an <ItemsControl> of ITEMS and disable the others)
    /// </summary>
    /// <example>
    /// How to use
    /// (You can reverse the returned Boolean by giving "ReverseBoolean" as ConverterParameter)
    /// <code>
    /// <UserControl>
    ///     <UserControl.IsEnabled>
    ///         <MultiBinding Converter="{StaticResource ItemsEqualToBoolean}">
    ///             <Binding Path="ITEM1_PROPERTY"/>
    ///             <Binding Path="ITEM2_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class ItemsEqualToBoolean : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2)
            {
                throw new ArgumentException("ItemsEqualToBoolean converter needs 2 values [item1, item2] !", "values");
            }

            var equal = true;
            var notEqual = false;
            if ((string) parameter == "ReverseBoolean")
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
            throw new InvalidOperationException("ItemsEqualToBoolean is a One-Way converter only !");
        }
    }
}