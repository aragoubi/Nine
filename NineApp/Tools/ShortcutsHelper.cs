using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Nine.Tools
{
    /// <summary>
    /// ShortcutHelper provide a way to declare properly ours shorcuts for this app
    /// </summary>
    internal class ShortcutsHelper
    {
        private static bool isRegister;

        private static readonly Dictionary<string, List<KeyEventHandler>> keyboardEvents =
            new Dictionary<string, List<KeyEventHandler>>();

        private static readonly Dictionary<string, List<MouseWheelEventHandler>> mouseWheelEvents =
            new Dictionary<string, List<MouseWheelEventHandler>>();

        /// <summary>
        /// Register new event for the given page
        /// </summary>
        /// <param name="page">Name of the current page</param>
        /// <param name="keyboard">Handler to call when a KeyDown event is triggered</param>
        /// <param name="mousewheel">Handler to call when a MouseWheel event is triggered</param>
        public static void RegisterShortcuts(string page, KeyEventHandler keyboard, MouseWheelEventHandler mousewheel)
        {
            if (!isRegister)
            {
                isRegister = true;
                Application.Current.MainWindow.PreviewKeyDown += App_PreviewKeyDown;
                Application.Current.MainWindow.PreviewMouseWheel += App_PreviewMouseWheel;
            }
            if (!keyboardEvents.ContainsKey(page))
            {
                keyboardEvents.Add(page, new List<KeyEventHandler>());
            }
            if (!mouseWheelEvents.ContainsKey(page))
            {
                mouseWheelEvents.Add(page, new List<MouseWheelEventHandler>());
            }
            if (keyboard != null)
            {
                keyboardEvents[page].Add(keyboard);
            }
            if (mousewheel != null)
            {
                mouseWheelEvents[page].Add(mousewheel);
            }
        }

        /// <summary>
        /// Called when a KeyDown event is triggered, recursively call all registered events for the KeyDown event
        /// </summary>
        private static void App_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!keyboardEvents.ContainsKey(Catalog.Instance.CurrentPageName))
            {
                return;
            }
            foreach (var keyboardEvent in keyboardEvents[Catalog.Instance.CurrentPageName])
            {
                keyboardEvent.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Called when a MouseWheel event is triggered, recursively call all registered events for the MouseWheel event
        /// </summary>
        private static void App_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!mouseWheelEvents.ContainsKey(Catalog.Instance.CurrentPageName))
            {
                return;
            }
            foreach (var mouseWheelEvent in mouseWheelEvents[Catalog.Instance.CurrentPageName])
            {
                mouseWheelEvent.Invoke(sender, e);
            }
        }
    }
}