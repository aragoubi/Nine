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
    ///     Logique d'interaction pour FreeNotesPage.xaml
    /// </summary>
    public partial class FreeNotesPage : Page
    {
        private List<TouchDevice> touchDevices = new List<TouchDevice>();
        private Point touchOrigin;

        public FreeNotesPage()
        {
            InitializeComponent();
            LessonPage = (Catalog.Instance.GetPage("MainPage") as MainPage).LessonPage.DataContext as Lesson;

            LayerStackDC = new LayerStack(Data.Instance.Lesson.GlobalNotes, Data.Instance.Lesson);
            LayerStackDC.Init(LessonPage);
            LayerStackDC.Viewport.SetComponents(LessonContainer, LessonContent, DrawingAreaBound, RadialMenu);

            appBar = AppBar.DataContext as AppBarState;

            DataContext = this;
        }

        public Lesson LessonPage { get; set; }
        public LayerStack LayerStackDC { get; set; }
        private AppBarState appBar { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShortcutsHelper.RegisterShortcuts("FreeNotesPage", Window_KeyDown, Window_MouseWheel);

            // Scrolls to current thumb inside the SideBar
            var lessonView = (Catalog.Instance.GetPage("MainPage") as MainPage);
            (LessonSidebar.ThumbsContainer.Parent as ScrollViewer).ScrollToVerticalOffset(
                (lessonView.LessonSidebar.ThumbsContainer.Parent as ScrollViewer).VerticalOffset);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Key down get triggered only outside of a Text field
            if (!Keyboard.FocusedElement.GetType().IsSubclassOf(typeof (TextBoxBase)))
            {
                // Will triggers lesson zoom, lesson translation or slide forward/backward
                LessonPage.AppKeyDown(e);
                LayerStackDC.Viewport.AppKeyDown(e);
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Will triggers lesson zoom
            LayerStackDC.Viewport.AppMouseWheel(e);
        }

        private void LessonContent_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Opens RadialMenu
            LayerStackDC.OpenRadialMenu(LayerStackDC.Viewport.GetViewport(), e.GetPosition(LessonContainer),
                RadialMenuState.Levels.Main);
        }

        private async void LessonContainer_TouchDown(object sender, TouchEventArgs e)
        {
            // Forward touch events to container
            //FrameworkElement control = sender as FrameworkElement;
            //control.CaptureTouch(e.TouchDevice);
            if (LayerStackDC.CurrentState.FingerInkingEnabled)
            {
                LessonContainer.IsContentManipulationEnabled = false;
                return;
            }

            // Saves current touch information
            //touchDevices.Add(e.TouchDevice);
            touchOrigin = e.GetTouchPoint(LessonContainer).Position;

            // Disable manipulation/drawing
            //LessonContainer.IsContentManipulationEnabled = false;
            LayerStackDC.ContainerTouchDown();

            // If touch-and-hold (within 10px radius circle, over 500ms)
            if (await TouchHelper.TouchHold(e, LessonContainer, 500, 10))
            {
                // Opens RadialMenu
                LessonContainer.IsContentManipulationEnabled = false;
                LayerStackDC.OpenRadialMenu(LayerStackDC.Viewport.GetViewport(),
                    e.GetTouchPoint(LessonContainer).Position, RadialMenuState.Levels.Main);

                // Un-forward touch events to container
                //control.ReleaseAllTouchCaptures();
            }
        }

        private void LessonContainer_TouchMove(object sender, TouchEventArgs e)
        {
            if (LayerStackDC.CurrentState.FingerInkingEnabled)
            {
                LessonContainer.IsContentManipulationEnabled = false;
                return;
            }

            // Verification to triggers this only one time
            if (!LessonContainer.IsContentManipulationEnabled && !LayerStackDC.RadialMenuState.IsOpen)
            {
                // If the manipulation is more than 10px expansion
                var touchCurrent = e.GetTouchPoint(LessonContainer).Position;
                if (TouchHelper.Distance(touchOrigin, touchCurrent) > 10)
                {
                    // Re-enable manipulation/drawing
                    //LayerStackDC.ContainerTouchMove();
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
            if (LayerStackDC.CurrentState.FingerInkingEnabled)
            {
                LessonContainer.IsContentManipulationEnabled = true;
                return;
            }

            // Re-enable manipulation/drawing
            LayerStackDC.ContainerTouchUp();
            LessonContainer.IsContentManipulationEnabled = true;
        }

        private void LessonContainer_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Will triggers lesson manipulations (to simulate pinch/move => zoom/scroll)
            LayerStackDC.Viewport.ContainerManipulationDelta(e);
        }

        private async void LessonContainer_StylusDown(object sender, StylusDownEventArgs e)
        {
            // If touch-and-hold (within 9px radius circle, over 700ms)
            if (await TouchHelper.StylusHold(e, LessonContainer, 700, 9))
            {
                // Opens RadialMenu
                LayerStackDC.OpenRadialMenu(LayerStackDC.Viewport.GetViewport(), e.GetPosition(LessonContainer),
                    RadialMenuState.Levels.Main);
            }
        }

        private void Page_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            (Application.Current.MainWindow.DataContext as NineManagement).CloseFlyout();
        }
    }
}