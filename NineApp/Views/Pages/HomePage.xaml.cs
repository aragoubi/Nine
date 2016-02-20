using System.Windows.Controls;
using Nine.ViewModels.Controls;

namespace Nine.Views.Pages
{
    /// <summary>
    ///     Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = new LessonStack();
            Loaded += (DataContext as LessonStack).ExistingLessons;
        }
    }
}