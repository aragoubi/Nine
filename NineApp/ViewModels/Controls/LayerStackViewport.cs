using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Nine.MVVM;
using Nine.Tools;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Describe the parent-component of the LayerStack who provide details for the drawing area
    /// </summary>
    public class LayerStackViewport : BaseViewModel
    {
        private readonly LayerStack layerStack;
        private readonly double maximumScale = 4.0;
        private readonly double mininumScale = 0.3;
        private double _horizontalOffset;
        private double _scale = 1;
        private double _verticalOffset;
        private FrameworkElement drawingAreaComponent;
        private ScrollViewer lessonContainerComponent;
        private FrameworkElement lessonContentComponent;
        private FrameworkElement radialMenuComponent;

        public LayerStackViewport(LayerStack layerStack)
        {
            this.layerStack = layerStack;
        }

        public double Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double HorizontalOffset
        {
            get { return _horizontalOffset; }
            set
            {
                if (_horizontalOffset != value)
                {
                    _horizontalOffset = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double VerticalOffset
        {
            get { return _verticalOffset; }
            set
            {
                if (_verticalOffset != value)
                {
                    _verticalOffset = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void CenterScrollViewer(FrameworkElement controlToCenter = null)
        {
            if (controlToCenter == null)
            {
                HorizontalOffset = lessonContainerComponent.ScrollableWidth/2.0;
                VerticalOffset = lessonContainerComponent.ScrollableWidth/2.0;
                return;
            }

            controlToCenter.UpdateLayout();

            if (controlToCenter.ActualWidth < lessonContainerComponent.ViewportWidth)
            {
                // Horizontal center the content of the scrollviewer
                HorizontalOffset = (lessonContainerComponent.ScrollableWidth - controlToCenter.Margin.Right)/2.0;
            }
            else
            {
                // Left center the content of the scrollviewer (with 25px left-margin)
                HorizontalOffset = ((drawingAreaComponent.ActualWidth - controlToCenter.ActualWidth -
                                     controlToCenter.Margin.Right)/2.0) - 25;
            }

            if (controlToCenter.ActualHeight < lessonContainerComponent.ViewportHeight)
            {
                // Vertical center the content of the scrollviewer
                VerticalOffset = (lessonContainerComponent.ScrollableHeight - controlToCenter.Margin.Right)/2.0;
            }
            else
            {
                // Top center the content of the scrollviewer (with 25px top-margin)
                VerticalOffset = ((drawingAreaComponent.ActualHeight - controlToCenter.ActualHeight -
                                   controlToCenter.Margin.Right)/2.0) - 25;
            }
        }

        public void ZoomIn(bool relativeToCenter = false)
        {
            Zoom(1.2, relativeToCenter);
        }

        public void ZoomOut(bool relativeToCenter = false)
        {
            Zoom(0.8, relativeToCenter);
        }

        public void Zoom(double delta, bool relativeToCenter = false)
        {
            var newScale = Scale*delta;
            newScale = Math.Max(mininumScale, newScale);
            newScale = Math.Min(maximumScale, newScale);
            Scale = newScale;

            if (relativeToCenter)
            {
                var viewport = GetViewport();
                ZoomTranslate(new Point(viewport.Width/2, viewport.Height/2), new Vector(0, 0));
            }
        }

        public void ZoomReset()
        {
            Scale = 1;
        }

        public void ZoomTranslate(Point originPoint, Vector deltaTranslate)
        {
            var translatedPoint = lessonContainerComponent.TranslatePoint(originPoint, lessonContentComponent);
            translatedPoint.X = translatedPoint.X*Scale;
            translatedPoint.Y = translatedPoint.Y*Scale;
            translatedPoint.X -= HorizontalOffset;
            translatedPoint.Y -= VerticalOffset;

            var horizontalDelta = (translatedPoint.X - originPoint.X) - deltaTranslate.X;
            var verticalDelta = (translatedPoint.Y - originPoint.Y) - deltaTranslate.Y;
            Translate(horizontalDelta, verticalDelta);
        }

        public void TranslateLeft(bool toTheEnd = false)
        {
            if (toTheEnd)
            {
                Translate(-lessonContainerComponent.ScrollableWidth, 0);
            }
            else
            {
                Translate(-20, 0);
            }
        }

        public void TranslateRight(bool toTheEnd = false)
        {
            if (toTheEnd)
            {
                Translate(lessonContainerComponent.ScrollableWidth, 0);
            }
            else
            {
                Translate(20, 0);
            }
        }

        public void TranslateUp(bool toTheEnd = false)
        {
            if (toTheEnd)
            {
                Translate(0, -lessonContainerComponent.ScrollableHeight);
            }
            else
            {
                Translate(0, -20);
            }
        }

        public void TranslateDown(bool toTheEnd = false)
        {
            if (toTheEnd)
            {
                Translate(0, lessonContainerComponent.ScrollableHeight);
            }
            else
            {
                Translate(0, 20);
            }
        }

        public void Translate(double horizontalDelta, double verticalDelta)
        {
            var newHorizontalOffset = HorizontalOffset + horizontalDelta;
            var newVerticalOffset = VerticalOffset + verticalDelta;
            newHorizontalOffset = Math.Max(0, newHorizontalOffset);
            newHorizontalOffset = Math.Min(lessonContainerComponent.ScrollableWidth, newHorizontalOffset);
            newVerticalOffset = Math.Max(0, newVerticalOffset);
            newVerticalOffset = Math.Min(lessonContainerComponent.ScrollableHeight, newVerticalOffset);

            HorizontalOffset = newHorizontalOffset;
            VerticalOffset = newVerticalOffset;
        }

        internal Rect GetViewport()
        {
            return new Rect(lessonContainerComponent.HorizontalOffset, lessonContainerComponent.VerticalOffset,
                lessonContainerComponent.ViewportWidth, lessonContainerComponent.ViewportHeight);
        }

        internal Rect GetDrawingArea()
        {
            return new Rect(drawingAreaComponent.TranslatePoint(new Point(0, 0), lessonContentComponent),
                new Size(drawingAreaComponent.ActualWidth, drawingAreaComponent.ActualHeight));
        }

        internal Point GetRadialMenuToDrawingAreaPoint()
        {
            var radialMenuRadius = 150;
            return radialMenuComponent.TranslatePoint(new Point(radialMenuRadius, radialMenuRadius),
                drawingAreaComponent);
        }

        internal void SetComponents(ScrollViewer lessonContainer, FrameworkElement lessonContent,
            FrameworkElement drawingAreaBound, FrameworkElement radialMenu)
        {
            lessonContainerComponent = lessonContainer;
            lessonContentComponent = lessonContent;
            drawingAreaComponent = drawingAreaBound;
            radialMenuComponent = radialMenu;
        }

        /*internal void AnimateScrollViewer(int x, int y)
        {
            var scrollViewer = (lessonContainerComponent as Nine.Views.Controls.TouchScrollViewer);
            var keyTime =  KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(750));
            var keySpline = new KeySpline(0.25, 1, 0.05, 1);

            if (x != 0)
            {
                var xAnimation = new DoubleAnimationUsingKeyFrames();
                var newXOffset = HorizontalOffset + x;
                xAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(newXOffset, keyTime, keySpline));
                xAnimation.Completed += new System.EventHandler(AnimateScrollViewerCompleted);
                scrollViewer.BeginAnimation(Nine.Views.Controls.TouchScrollViewer.HorizontalOffsetProperty, xAnimation);
            }

            if (y != 0)
            {
                var yAnimation = new DoubleAnimationUsingKeyFrames();
                var newYOffset = VerticalOffset + y;
                yAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(newYOffset, keyTime, keySpline));
                yAnimation.Completed += new System.EventHandler(AnimateScrollViewerCompleted);
                scrollViewer.BeginAnimation(Nine.Views.Controls.TouchScrollViewer.VerticalOffsetProperty, yAnimation);
            }
        }

        private void AnimateScrollViewerCompleted(object sender, EventArgs e)
        {
            // Ne marche pas (Le TouchScrollViewer ne marche plus après ça
            var scrollViewer = (lessonContainerComponent as Nine.Views.Controls.TouchScrollViewer);
            scrollViewer.ScrollToHorizontalOffset(HorizontalOffset);
            scrollViewer.ScrollToVerticalOffset(VerticalOffset);
            RaisePropertyChanged("HorizontalOffset");
            RaisePropertyChanged("VerticalOffset");
            lessonContainerComponent.UpdateLayout();
        }*/

        //private double cumulativeDelta = 0;

        public void ContainerManipulationDelta(ManipulationDeltaEventArgs e)
        {
            // Finger inking disable manipulation
            if (layerStack.CurrentState.FingerInkingEnabled)
            {
                return;
            }

            //var delta = (e.DeltaManipulation.Scale.X - 1);
            //cumulativeDelta += delta;

            //if (Math.Abs(cumulativeDelta) > 0.05)
            {
                //Zoom(cumulativeDelta + 1);
                Zoom(e.DeltaManipulation.Scale.X);
                ZoomTranslate(e.ManipulationOrigin, e.DeltaManipulation.Translation);
                //cumulativeDelta = 0;
            }
        }

        public void AppMouseWheel(MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                e.Handled = true;
                if (e.Delta > 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
                ZoomTranslate(e.GetPosition(lessonContainerComponent), new Vector(0, 0));
            }
        }

        public void AppKeyDown(KeyEventArgs e)
        {
            var switchSucceed = true;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.NumPad0:
                    case Key.D0:
                        ZoomReset();
                        if (Catalog.Instance.CurrentPageName == "MainPage")
                        {
                            layerStack.lessonViewModel.CenterScrollViewer();
                        }
                        else
                        {
                            TranslateLeft(true);
                            TranslateUp(true);
                        }
                        break;
                    case Key.Add:
                    case Key.OemPlus:
                        ZoomIn(true);
                        break;
                    case Key.Subtract:
                    case Key.D6:
                        ZoomOut(true);
                        break;
                    case Key.Left:
                        TranslateLeft();
                        break;
                    case Key.Right:
                        TranslateRight();
                        break;
                    case Key.Up:
                        TranslateUp();
                        break;
                    case Key.Down:
                        TranslateDown();
                        break;
                    case Key.Home:
                        TranslateLeft(true);
                        break;
                    case Key.End:
                        TranslateRight(true);
                        break;
                    case Key.PageUp:
                        TranslateUp(true);
                        break;
                    case Key.PageDown:
                        TranslateDown(true);
                        break;
                    default:
                        switchSucceed = false;
                        break;
                }
            }

            if (switchSucceed)
            {
                e.Handled = true;
            }
        }
    }
}