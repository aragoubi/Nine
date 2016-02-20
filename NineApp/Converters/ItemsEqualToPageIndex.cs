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
    /// KEY_OF_THE_STYLE+"TeacherActive" if Binding value #1 is equal to the Binding value #3
    /// or
    /// KEY_OF_THE_STYLE+"BothActive" if Binding value #1 is equal to the Binding value #2 and to Binding value #3
    /// or
    /// KEY_OF_THE_STYLE if Binding value #1 is not equal to any other Binding values
    /// (Usefull to have a specific Style for a specific ITEM inside an <ItemsControl> of ITEMS)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <UserControl>
    ///     <UserControl.IsEnabled>
    ///         <MultiBinding Converter="{StaticResource ItemsEqualToPageIndex}" ConverterParameter="KEY_OF_THE_STYLE"}>
    ///             <Binding Path="ITEM_PROPERTY"/>
    ///             <Binding Path="ITEM_STUDENT_PROPERTY"/>
    ///             <Binding Path="ITEM_TEACHER_PROPERTY"/>
    ///         </MultiBinding>
    ///     </UserControl.IsEnabled>
    /// </UserControl>
    /// </code>
    /// </example>
    internal class ItemsEqualToPageIndex : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 3)
            {
                throw new ArgumentException(
                    "ItemsEqualToPageIndex converter needs 3 values [currentIndex, myIndex, teacherIndex] !", "values");
            }
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter", "ItemsEqualToPageIndex converter needs the parameter !");
            }

            var noOne = (string) parameter;
            var meOnly = (string) parameter + "Active";
            var teacherOnly = (string) parameter + "TeacherActive";
            var both = (string) parameter + "BothActive";

            var chosenStyle = noOne;
            if (values[0].Equals(values[1]))
            {
                chosenStyle = meOnly;
                if (values[0].Equals(values[2]))
                    chosenStyle = both;
            }
            else if (values[0].Equals(values[2]))
                chosenStyle = teacherOnly;

            return Application.Current.TryFindResource(chosenStyle);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("ItemsEqualToPageIndex is a One-Way converter only !");
        }
    }
}