using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the corresponding Style
    /// "ExerciseThumb" + STYLE_SUFIX_PROPERTY + "Active" if Binding value #1 is equal to the Binding value #2
    /// or
    /// "ExerciseThumb" + STYLE_SUFIX_PROPERTY if Binding value #1 is not equal to the Binding value #2
    /// (Usefull to have a specific Style for a specific ITEM inside an <ItemsControl> of ITEMS)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl>
    ///     <UserControl.IsEnabled>
    ///         <MultiBinding Converter="{StaticResource ExerciceKindToThumbStyle}" ConverterParameter="KEY_OF_THE_STYLE"}>
    ///             <Binding Path="ITEM1_PROPERTY"/>
    ///             <Binding Path="ITEM2_PROPERTY"/>
    ///             <Binding Path="STYLE_SUFIX_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class ExerciceKindToThumbStyle : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 3)
                throw new ArgumentException(
                    "ExerciceKindToThumbStyle converter needs 3 values [ExId, CurrentEx, Kind] !", "values");

            var active = "";
            if (values[0].Equals(values[1]))
                active = "Active";

            return Application.Current.TryFindResource("ExerciseThumb" + values[2] + active);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ExerciceKindToThumbStyle is a One-Way converter only !");
        }
    }
}