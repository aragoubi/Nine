using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using Nine.Tools;
using Nine.ViewModels.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Logique d'interaction pour LessonSelection.xaml
    /// </summary>
    public partial class LessonSelection : UserControl
    {
        public LessonSelection()
        {
            InitializeComponent();
        }

        private void Tile_MouseRightButtonUp(object sender, RoutedEventArgs e)
        {
            OpenFlyoutMenu();

            //Définit la leçon sur laquelle on vient d'ouvrir l'appbar
            var tile = sender as Tile;
            var idLesson = (int) tile.Tag;
            var lessonStack = Catalog.Instance.GetPage("HomePage").DataContext as LessonStack;
            ;
            lessonStack.IndexCurrentLesson = idLesson;
        }

        /// <summary>
        ///     Opens the appbar lesson.
        /// </summary>
        private void OpenFlyoutMenu()
        {
            var mainwindow = Application.Current.MainWindow as MetroWindow;
            var flyout = mainwindow.Flyouts.Items[1] as Flyout;
            if (flyout.IsOpen != true)
                flyout.IsOpen = !flyout.IsOpen;
        }

        /*private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            ViewModels.Controls.LessonStack lessonStack = DataContext as ViewModels.Controls.LessonStack;
            lessonStack.ValidateRenaming.Execute(null);
        }*/

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            var lessonStack = DataContext as LessonStack;
            lessonStack.LostFocus.Execute(null);
        }

        private void TextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.IsEnabled)
            {
                Dispatcher.BeginInvoke(
                    DispatcherPriority.ContextIdle,
                    new Action(delegate
                    {
                        textBox.Focus();
                        textBox.SelectAll();
                    })
                    );
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key == Key.Enter)
            {
                var lessonStack = DataContext as LessonStack;
                lessonStack.ValidateRenaming.Execute(null);
            }
        }
    }
}