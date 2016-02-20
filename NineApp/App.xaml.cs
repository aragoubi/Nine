using LinkIOcsharp;
using Nine.Tools;
using Nine.ViewModels.Pages;
using Nine.Views.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace Nine
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow currentWindow;
        LinkIOcsharp.LinkIO linkIO;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            linkIO = LinkIOImp.Instance;

            if (!Directory.Exists(Data.NineFolder))
            {
                Directory.CreateDirectory(Data.NineFolder);
            }

            currentWindow = new MainWindow();
#if START_ON_MAINPAGE
            // Connect user as a Teacher
            new UserConnection().Connection.Execute(0);

            // If a Lesson exists, go to MainPage
            var nineFiles = Directory.GetFiles(Data.NineFolder.ToString(), "*.nine", SearchOption.AllDirectories);
            if (nineFiles.Length > 0)
            {
                FileStream serializedLesson = new FileStream(nineFiles[0], FileMode.Open);
                Data.Instance.Lesson = (Lessons.Lesson)(new BinaryFormatter()).Deserialize(serializedLesson);
                serializedLesson.Close();
                Catalog.Instance.NavigateTo("MainPage");
            }
            // Else go to HomePage (to create a Lesson)
            else
            {
                Catalog.Instance.NavigateTo("HomePage");
            }
#else
            Catalog.Instance.NavigateTo("UserConnectionPage");
#endif
            currentWindow.Show();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            // To close Nine Threads if some exists
            //App.Current.Dispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Send);
            base.OnExit(e);
        }

        public void NavigateTo(Page page)
        {
            currentWindow.Content = page;
        }
    }
}



