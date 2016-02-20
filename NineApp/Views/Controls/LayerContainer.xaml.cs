using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Nine.Layers.Components.Draggables;
using Nine.Tools;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for LayerContent.xaml
    /// </summary>
    public partial class LayerContainer : UserControl
    {
        public LayerContainer()
        {
            InitializeComponent();
        }

        private bool IsBulletQuestionLayer()
        {
            var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);
            return Catalog.Instance.CurrentPageName == "QuestionsPage" && layer.Type == "Question" &&
                   layer.TypeQuestion == "Bullets" && Data.Instance.User.IsTeacher;
        }

        private bool IsQuestionPage()
        {
            var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);
            return Catalog.Instance.CurrentPageName == "QuestionsPage";
        }

        private void LayerContainer_Loaded(object sender, RoutedEventArgs e)
        {
            // Remove fake components (new one will be added)
            InkLayer.Children.Clear();
            DeleteSelectedComponents.Visibility = Visibility.Collapsed;

            //var layerStack = (InkLayer.Tag as ViewModels.Controls.LayerStack);
            var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);
            layer.PropertyChanged += OnPropertyChanged;

            ShortcutsHelper.RegisterShortcuts(Catalog.Instance.CurrentPageName, App_PreviewKeyDown, null);
        }

        private void BulletComponents_Loaded(object sender, RoutedEventArgs e)
        {
            // Only the teach can move Bullets
            if (IsBulletQuestionLayer())
            {
                // Allow to move Bullets with the InkCanvas logic
                InitializeFakeComponents(BulletComponents);
            }
        }

        private void DraggableComponents_Loaded(object sender, RoutedEventArgs e)
        {
            // Allow to move DraggableComponents with the InkCanvas logic
            InitializeFakeComponents(DraggableComponents);
        }

        private void ChartComponents_Loaded(object sender, RoutedEventArgs e)
        {
            // Allow to move ChartComponents with the InkCanvas logic
            InitializeFakeComponents(ChartComponents);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var layerStack = (InkLayer.Tag as ViewModels.Controls.LayerStack);
            //var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);

            // To update new DraggableComponent and Bullet for the InkCanvas logic
            if (e.PropertyName == "AddBulletComponent")
            {
                UpdateLayout();

                var addedIndex = layerStack.CurrentLayer.BulletComponents.Count - 1;
                var addedControl =
                    (FrameworkElement) BulletComponents.ItemContainerGenerator.ContainerFromIndex(addedIndex);
                var addedItem = layerStack.CurrentLayer.BulletComponents[addedIndex];

                AddFakeComponent(addedControl, addedItem);
            }
            else if (e.PropertyName == "AddDraggableComponent")
            {
                UpdateLayout();

                var addedIndex = layerStack.CurrentLayer.DraggableComponents.Count - 1;
                var addedControl =
                    (FrameworkElement) DraggableComponents.ItemContainerGenerator.ContainerFromIndex(addedIndex);
                var addedItem = layerStack.CurrentLayer.DraggableComponents[addedIndex];

                AddFakeComponent(addedControl, addedItem);
            }
            else if (e.PropertyName == "RemoveBulletComponent")
            {
                var elementsToDelete = new List<FrameworkElement>();
                foreach (var child in InkLayer.Children)
                {
                    var element = child as FrameworkElement;
                    var quizBullet = element.Tag as QuizBullet;
                    if (quizBullet != null && !layerStack.CurrentLayer.BulletComponents.Contains(quizBullet))
                    {
                        elementsToDelete.Add(element);
                    }
                }

                // Convert Iterator to List
                foreach (var element in elementsToDelete)
                {
                    InkLayer.Children.Remove(element);
                }
            }
            else if (e.PropertyName == "RemoveDraggableComponent")
            {
                var elementsToDelete = new List<FrameworkElement>();
                foreach (var child in InkLayer.Children)
                {
                    var element = child as FrameworkElement;
                    var draggableComponent = element.Tag as DraggableComponent;
                    if (draggableComponent != null &&
                        !layerStack.CurrentLayer.DraggableComponents.Contains(draggableComponent))
                    {
                        elementsToDelete.Add(element);
                    }
                }

                // Convert Iterator to List
                foreach (var element in elementsToDelete)
                {
                    InkLayer.Children.Remove(element);
                }
            }
            //else if (e.PropertyName == "IsCurrentLayer")
            //{
            //    // Unselect all component when changing of layer
            //    if (!layer.IsCurrentLayer)
            //    {
            //        // Doesn't work : Causes selection issues
            //        InkLayer.Select(null, null);
            //    }
            //}
        }

        private void InitializeFakeComponents(ItemsControl items)
        {
            var enumerator = items.ItemsSource.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                var control = (FrameworkElement) items.ItemContainerGenerator.ContainerFromIndex(i);
                AddFakeComponent(control, enumerator.Current);
                i++;
            }
        }

        private void AddFakeComponent(FrameworkElement control, object item)
        {
            if (control != null && item != null)
            {
                var fakeControl = new Rectangle();
                fakeControl.Tag = item;
                fakeControl.Fill = Brushes.Transparent;

                //InkCanvas.SetTop(fakeControl, item.Position.X);
                //InkCanvas.SetLeft(fakeControl, item.Position.Y);
                fakeControl.Width = control.ActualWidth;
                fakeControl.Height = control.ActualHeight;

                var positionBindingPrefix = "";
                if (typeof (DraggableComponent).IsAssignableFrom(item.GetType()))
                {
                    positionBindingPrefix = "Position.";
                }

                var leftBinding = new Binding(positionBindingPrefix + "X");
                leftBinding.Mode = BindingMode.TwoWay;
                leftBinding.Source = item;
                fakeControl.SetBinding(InkCanvas.LeftProperty, leftBinding);

                var rightBinding = new Binding(positionBindingPrefix + "Y");
                rightBinding.Mode = BindingMode.TwoWay;
                rightBinding.Source = item;
                fakeControl.SetBinding(InkCanvas.TopProperty, rightBinding);

                if (item.GetType() == typeof (TextFrame))
                {
                    var widthBinding = new Binding("Width");
                    widthBinding.Mode = BindingMode.TwoWay;
                    widthBinding.Source = item;
                    fakeControl.SetBinding(WidthProperty, widthBinding);

                    var heightBinding = new Binding("Height");
                    heightBinding.Mode = BindingMode.TwoWay;
                    heightBinding.Source = item;
                    fakeControl.SetBinding(HeightProperty, heightBinding);
                }
                else
                {
                    fakeControl.MinHeight = fakeControl.Height;
                    fakeControl.MinWidth = fakeControl.Width;
                    fakeControl.MaxHeight = fakeControl.Height;
                    fakeControl.MaxWidth = fakeControl.Width;
                }

                InkLayer.Children.Add(fakeControl);
            }
        }

        private void DeleteSelectedElements(ReadOnlyCollection<UIElement> elements, StrokeCollection strokes)
        {
            var layerStack = (InkLayer.Tag as ViewModels.Controls.LayerStack);
            var elementsToDelete = new List<FrameworkElement>();

            // Convert Iterator to List
            foreach (var element in elements)
            {
                elementsToDelete.Add((FrameworkElement) element);
            }

            // Remove DraggableComponents and QuizBullets
            foreach (var element in elementsToDelete)
            {
                var control = element;
                var item = control.Tag;

                // Only the teacher can remove the bullets
                if (item.GetType().IsSubclassOf(typeof (QuizBullet)) && Data.Instance.User.IsTeacher)
                {
                    layerStack.RemoveBullet((QuizBullet) item);
                }
                else if (item.GetType().IsSubclassOf(typeof (DraggableComponent)))
                {
                    layerStack.RemoveDraggableComponent((DraggableComponent) item);
                }
            }

            // Remove strokes
            foreach (var stroke in strokes)
            {
                InkLayer.Strokes.Remove(stroke);
            }

            DeleteSelectedComponents.Visibility = Visibility.Collapsed;
        }

        private void InkLayer_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
            var gestures = e.GetGestureRecognitionResults();
            var recognized = false;
            e.Cancel = true;
            e.Handled = true;

            // Gesture recognition
            var gestureBounds = e.Strokes.GetBounds();
            var position = new Point(gestureBounds.X + (gestureBounds.Width/2),
                gestureBounds.Y + (gestureBounds.Height/2));
            var layerStack = (InkLayer.Tag as ViewModels.Controls.LayerStack);
            var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);
            var i = 0;
            var maxSearch = 2;
            while (!recognized && i <= maxSearch && i < gestures.Count)
            {
                var gesture = gestures[i].ApplicationGesture;
                var confidence = gestures[i].RecognitionConfidence;

                // If the user is a Teacher on the QuestionsPage + Square/Circle => CheckBox/RadioButton
                if (IsBulletQuestionLayer() && gestureBounds.Width > 20 && gestureBounds.Width < 90 &&
                    confidence == RecognitionConfidence.Strong &&
                    (gesture == ApplicationGesture.Square || gesture == ApplicationGesture.Circle))
                {
                    // Add Bullet if recognize as Square or Circle
                    layerStack.AddNewBullet(position);

                    // Remove the Stroke for this gesture
                    recognized = true;
                    e.Cancel = false;
                    e.Handled = false;
                }

                // Square => TextBox
                else if (!IsQuestionPage() && gestureBounds.Width > 120 && confidence == RecognitionConfidence.Strong &&
                         gesture == ApplicationGesture.Square)
                {
                    // Add Bullet if recognize as Square or Circle
                    layerStack.AddNewTextBox(position);

                    // Remove the Stroke for this gesture
                    recognized = true;
                    e.Cancel = false;
                    e.Handled = false;
                }
                i++;
            }
        }

        private void ToggleButton_TouchDown(object sender, TouchEventArgs e)
        {
            var button = sender as ToggleButton;
            button.CaptureTouch(e.TouchDevice);
            e.Handled = true;
        }

        private void ToggleButton_TouchUp(object sender, TouchEventArgs e)
        {
            var button = sender as ToggleButton;
            button.ReleaseTouchCapture(e.TouchDevice);
            e.Handled = true;
        }

        private void App_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);
            var elements = InkLayer.GetSelectedElements();
            var strokes = InkLayer.GetSelectedStrokes();
            if (elements.Count > 0 || strokes.Count > 0)
            {
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                    (e.Key == Key.X || e.Key == Key.C || e.Key == Key.V))
                {
                    e.Handled = true;
                }
                else if (e.Key == Key.Delete && layer.IsCurrentLayer)
                {
                    DeleteSelectedElements(elements, strokes);
                    e.Handled = true;
                }
            }
        }

        private void DeleteSelectedComponents_Click(object sender, RoutedEventArgs e)
        {
            var elements = InkLayer.GetSelectedElements();
            var strokes = InkLayer.GetSelectedStrokes();
            if (elements.Count > 0 || strokes.Count > 0)
            {
                DeleteSelectedElements(elements, strokes);
            }
            e.Handled = true;
        }

        private void InkLayer_SelectionChanged(object sender, EventArgs e)
        {
            var layer = (InkLayer.DataContext as ViewModels.Controls.Layer);
            InkLayer.UpdateLayout();
            var bounds = InkLayer.GetSelectionBounds();
            if (bounds != null && !bounds.IsEmpty && bounds.Width > 0 && bounds.Height > 0)
            {
                var topRightCorner = new Point(bounds.X + bounds.Width, bounds.Y);
                DeleteSelectedComponents.SetValue(Canvas.LeftProperty, topRightCorner.X - 2);
                DeleteSelectedComponents.SetValue(Canvas.TopProperty, topRightCorner.Y - 23);
                DeleteSelectedComponents.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteSelectedComponents.Visibility = Visibility.Collapsed;
            }
        }
    }
}