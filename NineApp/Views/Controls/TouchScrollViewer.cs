using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for TouchScrollViewer.xaml
    /// </summary>
    [TemplatePart(Name = "PART_ScrollContentPresenterParent", Type = typeof (Grid))]
    [TemplatePart(Name = "PART_ScrollContentPresenter", Type = typeof (ScrollContentPresenter))]
    [TemplatePart(Name = "PART_VerticalScrollBar", Type = typeof (ScrollBar))]
    [TemplatePart(Name = "PART_HorizontalScrollBar", Type = typeof (ScrollBar))]
    public class TouchScrollViewer : ScrollViewer
    {
        public new static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof (double), typeof (TouchScrollViewer),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender,
                    OnHorizontalOffsetChanged));

        public new static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (TouchScrollViewer),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender,
                    OnVerticalOffsetChanged));

        public static readonly DependencyProperty IsContentManipulationEnabledProperty =
            DependencyProperty.RegisterAttached("IsContentManipulationEnabled", typeof (bool),
                typeof (TouchScrollViewer),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        private Grid part_scrollContentPresenterParent;

        static TouchScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (TouchScrollViewer),
                new FrameworkPropertyMetadata(typeof (TouchScrollViewer)));
        }

        public new double HorizontalOffset
        {
            get { return (double) GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public new double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public bool IsContentManipulationEnabled
        {
            get { return (bool) GetValue(IsContentManipulationEnabledProperty); }
            set { SetValue(IsContentManipulationEnabledProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Code to get the Template parts as instance member
            part_scrollContentPresenterParent = GetTemplateChild("PART_ScrollContentPresenterParent") as Grid;

            if (part_scrollContentPresenterParent == null)
            {
                throw new NullReferenceException(
                    "Template \"ScrollContentPresenterParent\" part in \"TouchScrollViewer\" is not available");
            }
        }

        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            if (e.HorizontalOffset != HorizontalOffset || e.VerticalOffset != VerticalOffset)
            {
                HorizontalOffset = e.HorizontalOffset;
                VerticalOffset = e.VerticalOffset;
                base.OnScrollChanged(e);
            }
        }

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as TouchScrollViewer;
            if (scrollViewer != null && e.NewValue != e.OldValue)
            {
                scrollViewer.ScrollToHorizontalOffset((double) e.NewValue);
            }
        }

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as TouchScrollViewer;
            if (scrollViewer != null && e.NewValue != e.OldValue)
            {
                scrollViewer.ScrollToVerticalOffset((double) e.NewValue);
            }
        }

        internal void ContentCaptureTouch(TouchDevice touch)
        {
            part_scrollContentPresenterParent.CaptureTouch(touch);
        }

        internal void ContentReleaseTouchCapture(TouchDevice touch)
        {
            part_scrollContentPresenterParent.ReleaseTouchCapture(touch);
        }

        internal void ContentReleaseAllTouchCaptures()
        {
            part_scrollContentPresenterParent.ReleaseAllTouchCaptures();
        }
    }
}