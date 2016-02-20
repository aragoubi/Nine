using System.Windows.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for Document.xaml
    /// </summary>
    public partial class Lesson : UserControl
    {
        public Lesson()
        {
            InitializeComponent();
            DataContext = new ViewModels.Controls.Lesson();
        }
    }
}