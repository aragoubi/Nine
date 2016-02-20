using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Nine.Layers.Components.Strokes.DA;
using Color = System.Windows.Media.Color;
using NineInk = Nine.Layers.Components.Strokes;
using WindowsInk = System.Windows.Ink;

namespace Nine.Tools
{
    /// <summary>
    /// Provide a quick-way to go through the Nine implementation of strokes and the ".NET" way.
    /// </summary>
    internal static class StrokeConverter
    {
        public static WindowsInk.Stroke ToWindowsStroke(NineInk.Stroke nineStroke)
        {
            var points = new StylusPointCollection();

            foreach (var point in nineStroke.Points)
                points.Add(new StylusPoint(point.X, point.Y, point.Pressure));

            var drwAttr = new WindowsInk.DrawingAttributes();
            var c = new Color();
            c.R = nineStroke.DrawingAttributes.Color.R;
            c.G = nineStroke.DrawingAttributes.Color.G;
            c.B = nineStroke.DrawingAttributes.Color.B;
            c.A = nineStroke.DrawingAttributes.Color.A;
            drwAttr.Color = c;

            switch (nineStroke.DrawingAttributes.Brush.Name)
            {
                case "Rectangle":
                    drwAttr.StylusTip = WindowsInk.StylusTip.Rectangle;
                    break;
                case "Ellipse":
                default:
                    drwAttr.StylusTip = WindowsInk.StylusTip.Ellipse;
                    break;
            }
            drwAttr.Height = nineStroke.DrawingAttributes.Height;
            drwAttr.Width = nineStroke.DrawingAttributes.Width;
            drwAttr.IsHighlighter = nineStroke.DrawingAttributes.IsHighlighter;

            return new WindowsInk.Stroke(points, drwAttr);
        }

        public static NineInk.Stroke ToNineStroke(WindowsInk.Stroke windowsStroke)
        {
            var points = new List<NineInk.StrokePoint>();

            foreach (var point in windowsStroke.StylusPoints)
                points.Add(new NineInk.StrokePoint(point.X, point.Y, point.PressureFactor));

            var drwAttr = new DrawingAttributes();

            drwAttr.Color.R = windowsStroke.DrawingAttributes.Color.R;
            drwAttr.Color.G = windowsStroke.DrawingAttributes.Color.G;
            drwAttr.Color.B = windowsStroke.DrawingAttributes.Color.B;
            drwAttr.Color.A = windowsStroke.DrawingAttributes.Color.A;

            switch (windowsStroke.DrawingAttributes.StylusTip.ToString())
            {
                case "Rectangle":
                    drwAttr.Brush = Brushes.Rectangle;
                    break;
                case "Ellipse":
                default:
                    drwAttr.Brush = Brushes.Ellipse;
                    break;
            }
            drwAttr.Height = windowsStroke.DrawingAttributes.Height;
            drwAttr.Width = windowsStroke.DrawingAttributes.Width;
            drwAttr.IsHighlighter = windowsStroke.DrawingAttributes.IsHighlighter;

            return new NineInk.Stroke(points, drwAttr);
        }

        public static WindowsInk.StrokeCollection ToWindowsStrokes(Collection<NineInk.Stroke> nineStrokes)
        {
            var windowsStrokes = new WindowsInk.StrokeCollection();

            foreach (var stroke in nineStrokes)
                windowsStrokes.Add(ToWindowsStroke(stroke));

            return windowsStrokes;
        }

        public static Collection<NineInk.Stroke> ToNineStrokes(WindowsInk.StrokeCollection windowsStrokes)
        {
            var nineStrokes = new Collection<NineInk.Stroke>();

            foreach (var stroke in windowsStrokes)
                nineStrokes.Add(ToNineStroke(stroke));

            return nineStrokes;
        }
    }
}