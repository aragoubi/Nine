using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Nine.Tools
{
    /// <summary>
    /// Provide some methods to improve touch gestures
    /// </summary>
    internal class TouchHelper
    {
        /// <summary>
        /// Returns an awaitable Task that transform a TouchDown event in a TouchDown&Hold event
        /// </summary>
        /// <param name="originalEvent">Original TouchDown event</param>
        /// <param name="element">Original touched Control</param>
        /// <param name="msDuration">Duration before considering the TouchDown event as a TouchDown&Hold event</param>
        /// <param name="pxDelta">Circle radius in witch the TouchDown event must stay to be considered as a TouchDown&Hold event</param>
        /// <returns>Returns an awaitable Task that returns a Boolean</returns>
        public static Task<bool> TouchHold(TouchEventArgs originalEvent, FrameworkElement element, int msDuration,
            int pxDelta)
        {
            var originalPosition = originalEvent.GetTouchPoint(element).Position;
            var task = new TaskCompletionSource<bool>();
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(msDuration);

            EventHandler<TouchEventArgs> touchUpHandler = (o, e) =>
            {
                timer.Stop();
                if (task.Task.Status == TaskStatus.Running)
                {
                    task.SetResult(false);
                }
            };

            EventHandler<TouchEventArgs> touchMoveHandler = (o, e) =>
            {
                var currentPosition = e.GetTouchPoint(element).Position;
                if (Distance(currentPosition, originalPosition) > pxDelta)
                {
                    timer.Stop();
                    if (task.Task.Status == TaskStatus.Running)
                    {
                        task.SetResult(false);
                    }
                }
            };

            element.PreviewTouchUp += touchUpHandler;
            element.PreviewTouchMove += touchMoveHandler;

            timer.Tick += delegate
            {
                element.PreviewTouchUp -= touchUpHandler;
                element.PreviewTouchUp -= touchMoveHandler;
                timer.Stop();
                task.SetResult(true);
            };

            timer.Start();
            return task.Task;
        }

        /// <summary>
        /// Returns an awaitable Task that transform a StylusDown event in a StylusDown&Hold event
        /// </summary>
        /// <param name="originalEvent">Original StylusDown event</param>
        /// <param name="element">Original touched Control</param>
        /// <param name="msDuration">Duration before considering the StylusDown event as a StylusDown&Hold event</param>
        /// <param name="pxDelta">Circle radius in witch the StylusDown event must stay to be considered as a StylusDown&Hold event</param>
        /// <returns>Returns an awaitable Task that returns a Boolean</returns>
        public static Task<bool> StylusHold(StylusDownEventArgs originalEvent, FrameworkElement element, int msDuration,
            int pxDelta)
        {
            var originalPosition = originalEvent.GetPosition(element);
            var task = new TaskCompletionSource<bool>();
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(msDuration);

            StylusEventHandler stylusUpHandler = (o, e) =>
            {
                timer.Stop();
                if (task.Task.Status == TaskStatus.Running)
                {
                    task.SetResult(false);
                }
            };

            StylusEventHandler stylusMoveHandler = (o, e) =>
            {
                var currentPosition = e.GetPosition(element);
                if (Distance(currentPosition, originalPosition) > pxDelta)
                {
                    timer.Stop();
                    if (task.Task.Status == TaskStatus.Running)
                    {
                        task.SetResult(false);
                    }
                }
            };

            element.PreviewStylusUp += stylusUpHandler;
            element.PreviewStylusMove += stylusMoveHandler;

            timer.Tick += delegate
            {
                element.PreviewStylusUp -= stylusUpHandler;
                element.PreviewStylusMove -= stylusMoveHandler;
                timer.Stop();
                task.SetResult(true);
            };

            timer.Start();
            return task.Task;
        }

        /// <summary>
        /// Computes the distance between the Point "point1" and the Point "point2"
        /// </summary>
        /// <param name="point1">The first Point</param>
        /// <param name="point2">The second Point</param>
        /// <returns>Returns a Double that represents the distance between the two given Points</returns>
        public static double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(point1.X - point2.X), 2) + Math.Pow(Math.Abs(point1.Y - point2.Y), 2));
        }
    }
}