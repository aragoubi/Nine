using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Nine.Tools;
using Nine.ViewModels.Controls;
using Nine.ViewModels.Windows;

namespace Nine.Views.Pages
{
    /// <summary>
    ///     Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly LayerStack layerStack;
        private readonly Lesson lessonPage;
        private AppBarState appBar;
        private List<TouchDevice> touchDevices = new List<TouchDevice>();
        private Point touchOrigin;

        public MainPage()
        {
            InitializeComponent();
            layerStack = LayerStack.DataContext as LayerStack;
            lessonPage = LessonPage.DataContext as Lesson;
            appBar = AppBar.DataContext as AppBarState;

            lessonPage.SetComponents(LessonPage, LessonSidebar.ThumbsContainer);
            layerStack.Viewport.SetComponents(LessonContainer, LessonContent, DrawingAreaBound, RadialMenu);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShortcutsHelper.RegisterShortcuts("MainPage", Window_KeyDown, Window_MouseWheel);

            var currentThumb =
                LessonSidebar.ThumbsContainer.ItemContainerGenerator.ContainerFromIndex(lessonPage.CurrentPageIndex) as
                    ContentPresenter;
            currentThumb.BringIntoView();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Key down get triggered only outside of a Text field
            if (!Keyboard.FocusedElement.GetType().IsSubclassOf(typeof (TextBoxBase)))
            {
                // Will triggers lesson zoom, lesson translation or slide forward/backward
                lessonPage.AppKeyDown(e);
                layerStack.Viewport.AppKeyDown(e);
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Will triggers lesson zoom
            layerStack.Viewport.AppMouseWheel(e);
        }

        private void LessonContainer_Loaded(object sender, RoutedEventArgs e)
        {
            // To center the lesson page after it loads
            layerStack.Viewport.CenterScrollViewer(LessonPage);
        }

        private void LessonContent_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Opens RadialMenu
            layerStack.OpenRadialMenu(layerStack.Viewport.GetViewport(), e.GetPosition(LessonContainer),
                RadialMenuState.Levels.Main);
        }

        private async void LessonContainer_TouchDown(object sender, TouchEventArgs e)
        {
            // Forward touch events to container
            //FrameworkElement control = sender as FrameworkElement;
            //control.CaptureTouch(e.TouchDevice);
            if (layerStack.CurrentState.FingerInkingEnabled)
            {
                LessonContainer.IsContentManipulationEnabled = false;
                return;
            }

            // Saves current touch information
            //touchDevices.Add(e.TouchDevice);
            touchOrigin = e.GetTouchPoint(LessonContainer).Position;

            // Disable manipulation/drawing
            //LessonContainer.IsContentManipulationEnabled = false;
            layerStack.ContainerTouchDown();

            // If touch-and-hold (within 10px radius circle, over 500ms)
            if (await TouchHelper.TouchHold(e, LessonContainer, 500, 10))
            {
                // Opens RadialMenu
                LessonContainer.IsContentManipulationEnabled = false;
                layerStack.OpenRadialMenu(layerStack.Viewport.GetViewport(), e.GetTouchPoint(LessonContainer).Position,
                    RadialMenuState.Levels.Main);

                // Un-forward touch events to container
                //control.ReleaseAllTouchCaptures();
            }
        }

        private void LessonContainer_TouchMove(object sender, TouchEventArgs e)
        {
            if (layerStack.CurrentState.FingerInkingEnabled)
            {
                LessonContainer.IsContentManipulationEnabled = false;
                return;
            }

            // Verification to triggers this only one time
            if (!LessonContainer.IsContentManipulationEnabled && !layerStack.RadialMenuState.IsOpen)
            {
                // If the manipulation is more than 10px expansion
                var touchCurrent = e.GetTouchPoint(LessonContainer).Position;
                if (TouchHelper.Distance(touchOrigin, touchCurrent) > 10)
                {
                    // Re-enable manipulation/drawing
                    //layerStack.ContainerTouchMove();
                    LessonContainer.IsContentManipulationEnabled = true;

                    // Un-forward touch events to container
                    //foreach (var device in touchDevices)
                    //{
                    //    LessonContainer.ContentCaptureTouch(device);
                    //}
                }
            }
        }

        private void LessonContainer_TouchUp(object sender, TouchEventArgs e)
        {
            // Un-forward touch events to container
            //FrameworkElement control = sender as FrameworkElement;
            //control.ReleaseTouchCapture(e.TouchDevice);
            //LessonContainer.ContentReleaseTouchCapture(e.TouchDevice);
            //LessonContainer.IsContentManipulationEnabled = false;
            //e.Handled = true;
            if (layerStack.CurrentState.FingerInkingEnabled)
            {
                LessonContainer.IsContentManipulationEnabled = true;
                return;
            }

            // Re-enable manipulation/drawing
            layerStack.ContainerTouchUp();
            LessonContainer.IsContentManipulationEnabled = true;
        }

        private void LessonContainer_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Will triggers lesson manipulations (to simulate pinch/move => zoom/scroll)
            layerStack.Viewport.ContainerManipulationDelta(e);
        }

        private async void LessonContainer_StylusDown(object sender, StylusDownEventArgs e)
        {
            // If touch-and-hold (within 9px radius circle, over 700ms)
            if (await TouchHelper.StylusHold(e, LessonContainer, 700, 9))
            {
                // Opens RadialMenu
                layerStack.OpenRadialMenu(layerStack.Viewport.GetViewport(), e.GetPosition(LessonContainer),
                    RadialMenuState.Levels.Main);
            }
        }

        private void Page_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            (Application.Current.MainWindow.DataContext as NineManagement).CloseFlyout();
        }

        private void LessonContent_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Data.Instance.User.IsTeacher)
                lessonPage.IsSync = false;
        }

        private void LessonContent_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (!Data.Instance.User.IsTeacher)
                lessonPage.IsSync = false;
        }
    }
}