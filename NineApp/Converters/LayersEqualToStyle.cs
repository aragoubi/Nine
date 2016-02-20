using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Style
    /// STYLE_PREFIX_PROPERTY + KEY_OF_THE_STYLE + "Active" if Binding value #1 is equal to the Binding value #2
    /// or
    /// STYLE_PREFIX_PROPERTY + KEY_OF_THE_STYLE if Binding value #1 is not equal to the Binding value #2
    /// (Usefull to have a specific Style for a specific ITEM inside an <ItemsControl> of ITEMS)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl>
    ///     <UserControl.IsEnabled>
    ///         <MultiBinding Converter="{StaticResource LayersEqualToStyle}" ConverterParameter="KEY_OF_THE_STYLE"}>
    ///             <Binding Path="ITEM1_PROPERTY"/>
    ///             <Binding Path="ITEM2_PROPERTY"/>
    ///             <Binding Path="STYLE_PREFIX_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class LayersEqualToStyle : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 3)
            {
                throw new ArgumentException(
                    "LayersEqualToStyle converter needs 3 values [layer1, layer2, layerType] !", "values");
            }
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter", "LayersEqualToStyle converter needs the parameter !");
            }

            var equal = (string) values[2] + (string) parameter + "Active";
            var notEqual = (string) values[2] + (string) parameter;

            if (values[0].Equals(values[1]))
            {
                return Application.Current.TryFindResource(equal);
            }
            return Application.Current.TryFindResource(notEqual);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("LayersEqualToStyle is a One-Way converter only !");
        }
    }
}