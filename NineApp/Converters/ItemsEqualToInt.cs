using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Integer
    /// 1 if Binding value #1 is equal to the Binding value #2
    /// or
    /// 0 if Binding value #1 is not equal to the Binding value #2
    /// (Usefull to have a specific ITEM inside an <ItemsControl> of ITEMS on the top of the others)
    /// </summary>
    /// <example>
    /// How to use
    /// (You can reverse the returned Integer by giving "ReverseInt" as ConverterParameter)
    /// <code>
    /// <UserControl>
    ///     <Panel.ZIndex>
    ///         <MultiBinding Converter="{StaticResource ItemsEqualToInt}">
    ///             <Binding Path="ITEM1_PROPERTY"/>
    ///             <Binding Path="ITEM2_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class ItemsEqualToInt : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2)
            {
                throw new ArgumentException("ItemsEqualToInt converter needs 2 values [item1, item2] !", "values");
            }

            var equal = 1;
            var notEqual = 0;
            if ((string) parameter == "ReverseInt")
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
            throw new InvalidOperationException("ItemsEqualToInt is a One-Way converter only !");
        }
    }
}