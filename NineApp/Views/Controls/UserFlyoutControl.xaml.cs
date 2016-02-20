using System.Windows.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Logique d'interaction pour UserFlyoutControl.xaml
    /// </summary>
    public partial class UserFlyoutControl : UserControl
    {
        public UserFlyoutControl(string name, bool isTeacher)
        {
            InitializeComponent();
            UserName = name;
            IsTeacher = isTeacher;
        }

        public UserFlyoutControl()
        {
            InitializeComponent();
            DataContext = Data.Instance;
        }

        /// <summary>
        ///     Obtient ou définit le nom de l'utilisateur
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is related to a teacher user.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is related to a teacher user; otherwise, <c>false</c>.
        /// </value>
        public bool IsTeacher { get; set; }
    }
}