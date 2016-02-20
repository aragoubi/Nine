using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Nine.Tools;
using NineInk = Nine.Layers.Components.Strokes;
using WindowsInk = System.Windows.Ink;

namespace Nine.Converters
{
    /// <summary>
    /// Gives the Windows.StrokeCollection (respectively Collection<Nine.Stroke>)
    /// corresponding to the given Collection<Nine.Stroke> (respectively Windows.StrokeCollection)
    /// Needed to convert from "Windows Models" to "Nine Models" (for serialization)
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// <InkCanvas Strokes="{Binding STROKES_PROPERTY, Converter={StaticResource ToStrokeCollection}, Mode=TwoWay}"/>
    /// </code>
    /// </example>
    internal class ToStrokeCollection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // From Nine Strokes to Windows Strokes (from Model to View)
            return StrokeConverter.ToWindowsStrokes(value as Collection<NineInk.Stroke>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // From Windows Strokes to Nine Strokes (from View to Model)
            return StrokeConverter.ToNineStrokes(value as WindowsInk.StrokeCollection);
        }
    }
}