using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Style
    /// KEY_OF_THE_STYLE+"Active" if Binding value #1 is equal to the Binding value #2
    /// or
    /// KEY_OF_THE_STYLE if Binding value #1 is not equal to the Binding value #2
    /// (Usefull to have a specific Style for a specific ITEM inside an <ItemsControl> of ITEMS)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl>
    ///     <UserControl.IsEnabled>
    ///         <MultiBinding Converter="{StaticResource ItemsEqualToStyle}" ConverterParameter="KEY_OF_THE_STYLE"}>
    ///             <Binding Path="ITEM1_PROPERTY"/>
    ///             <Binding Path="ITEM2_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class ItemsEqualToStyle : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2)
            {
                throw new ArgumentException("ItemsEqualToStyle converter needs 2 values [item1, item2] !", "values");
            }
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter", "ItemsEqualToStyle converter needs the parameter !");
            }

            var equal = (string) parameter + "Active";
            var notEqual = (string) parameter;

            if (values[0].Equals(values[1]))
            {
                return Application.Current.TryFindResource(equal);
            }
            return Application.Current.TryFindResource(notEqual);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ItemsEqualToStyle is a One-Way converter only !");
        }
    }
}