using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Nine.Tools
{
    /// <summary>
    /// Catalog is a singleton-class who provide a way to change page by keeping the old reference (useful with MasterSwitch)
    /// Also, we use it to refer to the datacontext of ours pages from off-context components or specific needs (YEvents).
    /// </summary>
    public class Catalog : IDisposable
    {
        private static Catalog _instance;
        private Dictionary<string, Page> _pages;

        private Catalog()
        {
        }

        public string CurrentPageName { get; private set; }

        public static Catalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Catalog();
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        public Dictionary<string, Page> Pages
        {
            get
            {
                if (_pages == null)
                    Pages = new Dictionary<string, Page>();
                return _pages;
            }
            private set { _pages = value; }
        }

        public bool IsMainPage
        {
            get { return "MainPage" == CurrentPageName; }
        }

        public void Dispose()
        {
            Instance = null;
        }

        public void NavigateTo(string pageName)
        {
            (Application.Current as App).NavigateTo(GetPage(pageName));
            CurrentPageName = pageName;
        }

        public Page GetPage(string pageName)
        {
            if (!Pages.ContainsKey(pageName))
                Pages.Add(
                    pageName,
                    (Page) Activator.CreateInstance(Type.GetType("Nine.Views.Pages." + pageName)));

            return Pages[pageName];
        }

        public void RemovePage(string pageName)
        {
            if (Pages.ContainsKey(pageName))
            {
                Pages.Remove(pageName);
            }
        }

        public object GetDataContext(string pageName)
        {
            if (!Pages.ContainsKey(pageName))
                throw new KeyNotFoundException("This page isn't instantiated");

            return Pages[pageName].DataContext;
        }

        internal void ReplaceAndGoTo(string pageName)
        {
            Pages[pageName] = (Page) Activator.CreateInstance(Type.GetType("Nine.Views.Pages." + pageName));
            NavigateTo(pageName);
        }

        internal void ReInit(string pageName)
        {
            Pages[pageName] = (Page) Activator.CreateInstance(Type.GetType("Nine.Views.Pages." + pageName));
        }

        internal bool Exists(string namePage)
        {
            return Pages.ContainsKey(namePage);
        }
    }
}