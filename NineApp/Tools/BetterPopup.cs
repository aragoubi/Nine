using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;


namespace Nine.Tools
{
    /// <summary>
    /// Popup with code to not be the topmost control
    /// </summary>
    public class BetterPopup : Popup
    {
        /// <summary>
        ///     Is Topmost dependency property
        /// </summary>
        public static readonly DependencyProperty IsTopmostProperty = DependencyProperty.Register("IsTopmost",
            typeof (bool), typeof (BetterPopup), new FrameworkPropertyMetadata(false, OnIsTopmostChanged));

        private bool _alreadyLoaded;
        private bool? _appliedTopMost;
        private Window _parentWindow;

        /// <summary>
        ///     ctor
        /// </summary>
        public BetterPopup()
        {
            Loaded += OnPopupLoaded;
            Unloaded += OnPopupUnloaded;
        }

        /// <summary>
        ///     Get/Set IsTopmost
        /// </summary>
        public bool IsTopmost
        {
            get { return (bool) GetValue(IsTopmostProperty); }
            set { SetValue(IsTopmostProperty, value); }
        }

        private void OnPopupLoaded(object sender, RoutedEventArgs e)
        {
            if (_alreadyLoaded)
                return;

            _alreadyLoaded = true;

            if (Child != null)
            {
                Child.AddHandler(PreviewMouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(OnChildPreviewMouseLeftButtonDown), true);
            }

            _parentWindow = Window.GetWindow(this);

            if (_parentWindow == null)
                return;

            _parentWindow.Activated += OnParentWindowActivated;
            _parentWindow.Deactivated += OnParentWindowDeactivated;
        }

        private void OnPopupUnloaded(object sender, RoutedEventArgs e)
        {
            if (_parentWindow == null)
                return;
            _parentWindow.Activated -= OnParentWindowActivated;
            _parentWindow.Deactivated -= OnParentWindowDeactivated;
        }

        private void OnParentWindowActivated(object sender, EventArgs e)
        {
            //Debug.WriteLine("Parent Window Activated");
            SetTopmostState(true);
        }

        private void OnParentWindowDeactivated(object sender, EventArgs e)
        {
            //Debug.WriteLine("Parent Window Deactivated");

            if (IsTopmost == false)
            {
                SetTopmostState(IsTopmost);
            }
        }

        private void OnChildPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Child Mouse Left Button Down");

            SetTopmostState(true);

            if (!_parentWindow.IsActive && IsTopmost == false)
            {
                _parentWindow.Activate();
                //Debug.WriteLine("Activating Parent from child Left Button Down");
            }
        }

        private static void OnIsTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var thisobj = (BetterPopup) obj;

            thisobj.SetTopmostState(thisobj.IsTopmost);
        }

        protected override void OnOpened(EventArgs e)
        {
            SetTopmostState(IsTopmost);
            base.OnOpened(e);
        }

        private void SetTopmostState(bool isTop)
        {
            // Donâ€™t apply state if itâ€™s the same as incoming state
            if (_appliedTopMost.HasValue && _appliedTopMost == isTop)
            {
                return;
            }

            if (Child == null)
                return;

            var hwndSource = (PresentationSource.FromVisual(Child)) as HwndSource;

            if (hwndSource == null)
                return;
            var hwnd = hwndSource.Handle;

            RECT rect;

            if (!GetWindowRect(hwnd, out rect))
                return;

            //Debug.WriteLine("setting z-order " + isTop);

            if (isTop)
            {
                SetWindowPos(hwnd, HWND_TOPMOST, rect.Left, rect.Top, (int) Width, (int) Height, TOPMOST_FLAGS);
            }
            else
            {
                // Z-Order would only get refreshed/reflected if clicking the
                // the titlebar (as opposed to other parts of the external
                // window) unless I first set the popup to HWND_BOTTOM
                // then HWND_TOP before HWND_NOTOPMOST
                SetWindowPos(hwnd, HWND_BOTTOM, rect.Left, rect.Top, (int) Width, (int) Height, TOPMOST_FLAGS);
                SetWindowPos(hwnd, HWND_TOP, rect.Left, rect.Top, (int) Width, (int) Height, TOPMOST_FLAGS);
                SetWindowPos(hwnd, HWND_NOTOPMOST, rect.Left, rect.Top, (int) Width, (int) Height, TOPMOST_FLAGS);
            }

            _appliedTopMost = isTop;
        }

        #region P/Invoke imports & definitions

#pragma warning disable 1591 //Xml-doc
#pragma warning disable 169 //Never used-warning
        // ReSharper disable InconsistentNaming
        // Imports etc. with their naming rules

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
            int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;

        private const uint SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_NOOWNERZORDER = 0x0200; /* Donâ€™t do owner Z ordering */
        private const uint SWP_NOSENDCHANGING = 0x0400; /* Donâ€™t send WM_WINDOWPOSCHANGING */

        private const uint TOPMOST_FLAGS =
            SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_NOREDRAW | SWP_NOSENDCHANGING;

        // ReSharper restore InconsistentNaming
#pragma warning restore 1591
#pragma warning restore 169

        #endregion
    }
}