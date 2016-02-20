using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Ink;
using Nine.Layers;
using Nine.Layers.Components.Draggables;
using Nine.Lessons.Contents;
using Nine.MVVM;
using Nine.Tools;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Mandatory class who proxify the *Layer implementation from the model with the requirements of the View
    /// </summary>
    public class Layer : BaseViewModel
    {
        private readonly AbstractLayer layerModel;
        private readonly LayerStack layerStack;
        private readonly Lessons.Lesson lessonModel;
        private ObservableCollection<BarChart> _charts;
        private bool _isRenaming;
        private ObservableCollection<SaliencyMap> _saliencyMaps;
        private StrokeCollection _strokes;

        public Layer(LayerStack layerStack, Lessons.Lesson lessonModel, AbstractLayer layerModel)
        {
            this.layerStack = layerStack;
            this.lessonModel = lessonModel;
            this.layerModel = layerModel;
            Strokes = StrokeConverter.ToWindowsStrokes(layerModel.Strokes);

            if (layerModel.GetType() == typeof (QuizAnswerLayer))
            {
                DisplayChart();
            }
            else if (layerModel.GetType() == typeof (GraphicalAnswerLayer))
            {
                DisplaySaliencyMap();
            }
        }

        public bool IsCurrentLayer
        {
            get { return layerStack.CurrentLayer == this; }
        }

        public bool IsRenameable
        {
            get { return layerModel.IsRenameable; }
        }

        public bool IsHideable
        {
            get { return layerModel.IsHideable; }
        }

        public bool IsDeletable
        {
            get { return layerModel.IsDeletable; }
        }

        public bool IsShareable
        {
            get { return layerModel.IsShareable; }
        }

        public bool IsInkable
        {
            get { return layerModel.IsInkable; }
        }

        public int UID
        {
            get { return layerModel.UID; }
        }

        public string Name
        {
            get { return layerModel.Name; }
            set
            {
                var newName = value.Trim();
                if (newName.Length > 0)
                {
                    layerModel.Content.RenameLayer(layerModel.UID, newName);
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsDisplayed
        {
            get { return layerModel.IsDisplayed; }
            set
            {
                layerModel.IsDisplayed = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRenaming
        {
            get { return _isRenaming; }
            set
            {
                _isRenaming = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsNotRenaming");
            }
        }

        public bool IsNotRenaming
        {
            get { return !_isRenaming; }
            set
            {
                _isRenaming = !value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsRenaming");
            }
        }

        public string Type
        {
            get
            {
                if (typeof (IQuestionLayer).IsAssignableFrom(layerModel.GetType()))
                {
                    return "Question";
                }
                if (typeof (IAnswerLayer).IsAssignableFrom(layerModel.GetType()))
                {
                    return "Answer";
                }

                return "Classic";
            }
        }

        public string TypeQuestion
        {
            get
            {
                if (typeof (QuizLayer).IsAssignableFrom(layerModel.GetType()))
                {
                    var quiz = (QuizLayer) layerModel;
                    var content = (QuizContent) quiz.Content;
                    //if (content.Mode == QuizMode.QCM)
                    //{
                    //    return "QCM";
                    //}
                    //else if (content.Mode == QuizMode.QCU)
                    //{
                    //    return "QCU";
                    //}
                    return "Bullets";
                }
                if (typeof (BasicLayer).IsAssignableFrom(layerModel.GetType()))
                {
                    return "Graphical";
                }

                return "None";
            }
        }

        public StrokeCollection Strokes
        {
            get
            {
                if (_strokes == null)
                {
                    Strokes = new StrokeCollection();
                }
                return _strokes;
            }
            set
            {
                _strokes = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DraggableComponent> DraggableComponents
        {
            get { return new ObservableCollection<DraggableComponent>(layerModel.Components); }
        }

        public ObservableCollection<QuizBullet> BulletComponents
        {
            get
            {
                if (typeof (QuizLayer).IsAssignableFrom(layerModel.GetType()))
                {
                    return new ObservableCollection<QuizBullet>(((QuizLayer) layerModel).Bullets);
                }
                return new ObservableCollection<QuizBullet>();
            }
        }

        public ObservableCollection<BarChart> Charts
        {
            get
            {
                if (_charts == null)
                {
                    _charts = new ObservableCollection<BarChart>();
                }
                return _charts;
            }
            set
            {
                _charts = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<SaliencyMap> SaliencyMaps
        {
            get
            {
                if (_saliencyMaps == null)
                {
                    _saliencyMaps = new ObservableCollection<SaliencyMap>();
                }
                return _saliencyMaps;
            }
            set
            {
                _saliencyMaps = value;
                RaisePropertyChanged();
            }
        }

        //internal void IsTheCurrent(bool isCurrentLayer)
        //{
        //    // IsCurrentLayer is auto-computed
        //    RaisePropertyChanged("IsCurrentLayer");
        //}

        internal void AddTag(string tag, Point center)
        {
            var anchoredTag = new AnchoredTag(new Layers.Components.Point(center.X, center.Y), layerModel.Link,
                lessonModel.GetTag(tag));
            layerModel.Components.Add(anchoredTag);

            RaisePropertyChanged("DraggableComponents");
            RaisePropertyChanged("AddDraggableComponent"); // To update the view through LayerContainer.xaml.cs
        }

        internal void AddTextBox(Point center)
        {
            var textFrame = new TextFrame(new Layers.Components.Point(center.X, center.Y), "Votre texte ici...");
            layerModel.Components.Add(textFrame);
            RaisePropertyChanged("DraggableComponents");
            RaisePropertyChanged("AddDraggableComponent"); // To update the view through LayerContainer.xaml.cs
        }

        internal void RemoveDraggableComponent(DraggableComponent draggableComponent)
        {
            if (draggableComponent.GetType() == typeof (AnchoredTag))
            {
                ((AnchoredTag) draggableComponent).Unset();
            }

            layerModel.Components.Remove(draggableComponent);
            RaisePropertyChanged("DraggableComponents");
            RaisePropertyChanged("RemoveDraggableComponent"); // To update the view through LayerContainer.xaml.cs
        }

        internal void AddBullet(int index, Point center)
        {
            // Already added by the parent datacontext (because Bullet are not managed by the Layer, but by the Exercise)
            // So, just inform that their is a new Bullet to display in the view
            RaisePropertyChanged("BulletComponents");
            RaisePropertyChanged("AddBulletComponent"); // To update the view through LayerContainer.xaml.cs
        }

        internal void RemoveBullet(QuizBullet quizBullet)
        {
            // Already removed by the parent datacontext (because Bullet are not managed by the Layer, but by the Exercise)
            // So, just inform that their is a Bullet has been deleted to remove it of the view
            RaisePropertyChanged("BulletComponents");
            RaisePropertyChanged("RemoveBulletComponent"); // To update the view through LayerContainer.xaml.cs
        }

        internal int GetLastBulletOffset()
        {
            var bullets = ((QuizLayer) layerModel).Bullets;
            if (bullets.Count == 0)
            {
                return 0;
            }
            return bullets[bullets.Count - 1].Offset;
        }

        public void Save()
        {
            layerModel.Strokes = StrokeConverter.ToNineStrokes(Strokes);
        }

        public void Delete()
        {
            layerModel.Content.DeleteLayer(layerModel.UID);
        }

        public void DisplayChart()
        {
            if (layerModel.GetType() == typeof (QuizAnswerLayer))
            {
                var answerLayer = (QuizAnswerLayer) layerModel;
                var filledBounds = layerStack.Layers[0].Value.Strokes.GetBounds();
                    // On the first layer ("Question" layer)
                answerLayer.BarChart.Position.X = 150;
                if (filledBounds.IsEmpty)
                {
                    answerLayer.BarChart.Position.Y = 50;
                }
                else
                {
                    answerLayer.BarChart.Position.Y = filledBounds.Y + filledBounds.Height;
                }
                Charts.Clear();
                Charts.Add(new BarChart(answerLayer));
            }
        }

        public void DisplaySaliencyMap()
        {
            if (layerModel.GetType() == typeof (GraphicalAnswerLayer))
            {
                var drawingArea = layerStack.Viewport.GetDrawingArea();
                var answerLayer = (GraphicalAnswerLayer) layerModel;
                SaliencyMaps.Clear();
                SaliencyMaps.Add(new SaliencyMap(answerLayer, drawingArea.Width, drawingArea.Height));
            }
        }
    }
}