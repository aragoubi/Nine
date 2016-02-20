using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for LayerSelection.xaml
    /// </summary>
    public partial class LayerSelection : UserControl
    {
        public LayerSelection()
        {
            InitializeComponent();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var layerStack = DataContext as ViewModels.Controls.LayerStack;
            if (!layerStack.AppBarState.IsOpen)
            {
                layerStack.AppBarState.Open.Execute(null);
                e.Handled = true;
            }
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

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var layerStack = DataContext as ViewModels.Controls.LayerStack;
            layerStack.ValidateRenameCurrentLayer.Execute(null);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layerStack = DataContext as ViewModels.Controls.LayerStack;
                layerStack.ValidateRenameCurrentLayer.Execute(null);
                e.Handled = true;
            }
        }
    }
}