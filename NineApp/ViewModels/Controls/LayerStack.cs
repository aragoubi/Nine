using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Nine.Layers;
using Nine.Layers.Components.Draggables;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;
using Nine.MVVM;
using Nine.Tools;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Component who allow to super-impose several layers
    /// </summary>
    public class LayerStack : BaseViewModel
    {
        #region ATTRIBUTES

        private readonly Lessons.Lesson lessonModel;
        public Holder holderModel;
        public Lesson lessonViewModel;
        private int _currentLayerIndex;
        private bool _isFullscreenMode;
        private ObservableCollection<KeyValuePair<int, Layer>> _layers;
        private StrokeCollection _pointerStrokes;
        private ICommand _addNewLayer;
        private ICommand _addNewTag;
        private ICommand _deleteCurrentLayer;
        private ICommand _showCurrentLayer;
        private ICommand _hideCurrentLayer;
        private ICommand _initiateRenameCurrentLayer;
        private ICommand _validateRenameCurrentLayer;
        private ICommand _shareCurrentLayer;
        private ICommand _setDrawingMode;
        private ICommand _setDrawingColor;
        private ICommand _setDrawingThickness;
        private ICommand _activateFingerInking;
        private ICommand _selectLayer;
        private ICommand _toggleFullscreen;
        private ICommand _closeRadialMenu;
        private ICommand _goToRadialMenu;
        private DrawingAttributes _drawingAttributes;
        private DrawingAttributes defaultHighlighter;
        private DrawingAttributes defaultPencil;
        private DrawingAttributes defaultPointer;
        private StylusShape _eraserShape;
        private RectangleStylusShape defaultStrokeEraserShape;
        private RectangleStylusShape defaultPointEraserShape;
        private LayerStackState _currentState;
        private LayerStackViewport _viewport;
        private AppBarState _appBarState;
        private readonly LayerStackState.States defaultLayerStackState = LayerStackState.States.DrawingAndManipulating;
        private readonly LayerStackState.InkingModes defaultLayerStackInkingMode = LayerStackState.InkingModes.Pencil;
        private RadialMenuState _radialMenuState;

        #endregion

        #region LAYERS PROPERTIES

        public LayerStackState CurrentState
        {
            get
            {
                if (_currentState == null)
                {
                    _currentState = new LayerStackState(defaultLayerStackState, defaultLayerStackInkingMode);
                }
                return _currentState;
            }
            private set
            {
                _currentState = value;
                RaisePropertyChanged();
            }
        }

        public RadialMenuState RadialMenuState
        {
            get
            {
                if (_radialMenuState == null)
                {
                    _radialMenuState = new RadialMenuState(RadialMenuState.Levels.None, new Point(150, 150));
                }
                return _radialMenuState;
            }
            private set
            {
                _radialMenuState = value;
                RaisePropertyChanged();
            }
        }

        public int CurrentLayerIndex
        {
            get { return _currentLayerIndex; }
            set
            {
                if (_layers != null && value >= 0 && value < _layers.Count)
                {
                    ForwardCurrentLayersToModel();
                    _currentLayerIndex = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("CurrentLayer");
                }
            }
        }

        public bool HasCurrentLayer
        {
            get
            {
                return _layers != null && _layers.Count > 0 && _currentLayerIndex >= 0 &&
                       _currentLayerIndex < _layers.Count;
            }
        }

        public Layer CurrentLayer
        {
            get
            {
                if (HasCurrentLayer)
                {
                    return Layers[CurrentLayerIndex].Value;
                }
                return null;
            }
        }

        public ObservableCollection<KeyValuePair<int, Layer>> Layers
        {
            get
            {
                if (_layers == null)
                {
                    _layers = new ObservableCollection<KeyValuePair<int, Layer>>();
                }
                return _layers;
            }
            set
            {
                ForwardCurrentLayersToModel();
                _layers = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CurrentLayer");
            }
        }

        public StrokeCollection PointerStrokes
        {
            get
            {
                if (_pointerStrokes == null)
                {
                    _pointerStrokes = new StrokeCollection();
                }
                return _pointerStrokes;
            }
            private set
            {
                _pointerStrokes = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFullscreenMode
        {
            get { return _isFullscreenMode; }
            set
            {
                _isFullscreenMode = value;
                RaisePropertyChanged();
            }
        }

        public Lesson LessonViewModel
        {
            get { return lessonViewModel; }
            set { lessonViewModel = value; }
        }

        #endregion

        #region DRAWING PROPERTIES

        public DrawingAttributes DrawingAttributes
        {
            get
            {
                if (_drawingAttributes == null)
                {
                    defaultPointEraserShape = new RectangleStylusShape(10, 10);
                    defaultStrokeEraserShape = new RectangleStylusShape(2, 2);

                    defaultPencil = new DrawingAttributes();
                    defaultPencil.Color = Colors.Black;
                    defaultPencil.FitToCurve = false;
                    defaultPencil.IsHighlighter = false;
                    defaultPencil.StylusTip = StylusTip.Ellipse;
                    defaultPencil.Height = 2;
                    defaultPencil.Width = 2;

                    // We can chose our own opacity by setting Color to Color.FromArgb(opacity, red, blue, green);
                    defaultHighlighter = new DrawingAttributes();
                    defaultHighlighter.Color = Colors.Yellow;
                    defaultHighlighter.FitToCurve = false;
                    defaultHighlighter.IsHighlighter = true;
                    defaultHighlighter.StylusTip = StylusTip.Ellipse;
                    defaultHighlighter.Height = 14;
                    defaultHighlighter.Width = 4;

                    defaultPointer = new DrawingAttributes();
                    defaultPointer.Color = Colors.Red;
                    defaultPointer.FitToCurve = false;
                    defaultPointer.IsHighlighter = true;
                    defaultPointer.StylusTip = StylusTip.Ellipse;
                    defaultPointer.Height = 4;
                    defaultPointer.Width = 4;

                    _drawingAttributes = defaultPencil;
                }
                return _drawingAttributes;
            }
            set
            {
                _drawingAttributes = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentDrawingColor
        {
            get { return DrawingAttributes.Color.ToString(); }
        }

        public StylusShape EraserShape
        {
            get
            {
                if (_eraserShape == null)
                {
                    _eraserShape = defaultPointEraserShape;
                }
                return _eraserShape;
            }
            set
            {
                _eraserShape = value;
                RaisePropertyChanged();
            }
        }

        public LayerStackViewport Viewport
        {
            get
            {
                if (_viewport == null)
                {
                    _viewport = new LayerStackViewport(this);
                }
                return _viewport;
            }
            private set
            {
                _viewport = value;
                RaisePropertyChanged();
            }
        }

        public AppBarState AppBarState
        {
            get
            {
                if (_appBarState == null)
                {
                    _appBarState = new AppBarState();
                }
                return _appBarState;
            }
            private set
            {
                _appBarState = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region COMMANDS

        public ICommand AddNewLayer
        {
            get
            {
                if (_addNewLayer == null)
                {
                    _addNewLayer = new RelayCommand(
                        () =>
                        {
                            var currentPageIndex = Catalog.Instance.CurrentPageName == "FreeNotesPage"
                                ? 0
                                : lessonViewModel.CurrentPageIndex;
                            var currentContent = holderModel.Contents[currentPageIndex];
                            var uid = currentContent.AddLayer();
                            Layers.Add(new KeyValuePair<int, Layer>(Layers.Count,
                                new Layer(this, lessonModel, currentContent.GetLayerByUid(uid))));
                            RaisePropertyChanged("Layers");
                            RaisePropertyChanged("CurrentLayer");
                        }
                        );
                }
                return _addNewLayer;
            }
        }

        public ICommand AddNewTag
        {
            get
            {
                if (_addNewTag == null)
                {
                    _addNewTag = new ParametrizedRelayCommand(
                        tag =>
                        {
                            if (HasCurrentLayer)
                            {
                                var tagRadius = 20;
                                var activeLayer = Layers[CurrentLayerIndex].Value;
                                var drawingArea = Viewport.GetDrawingArea();
                                var tagPosition = Viewport.GetRadialMenuToDrawingAreaPoint();

                                tagPosition.X = Math.Max(tagPosition.X, 0);
                                tagPosition.X = Math.Min(tagPosition.X, drawingArea.Width);

                                tagPosition.Y = Math.Max(tagPosition.Y, 0);
                                tagPosition.Y = Math.Min(tagPosition.Y, drawingArea.Width);

                                tagPosition.Offset(-tagRadius, -tagRadius); // To center Tag on itself

                                activeLayer.AddTag((string) tag, tagPosition);
                            }
                            RadialMenuState.Close();
                        }
                        );
                }
                return _addNewTag;
            }
        }

        public ICommand DeleteCurrentLayer
        {
            get
            {
                if (_deleteCurrentLayer == null)
                {
                    _deleteCurrentLayer = new RelayCommand(
                        () =>
                        {
                            if (HasCurrentLayer)
                            {
                                CurrentLayer.Delete();
                                Layers.RemoveAt(CurrentLayerIndex);
                                var tmpLayers = new ObservableCollection<KeyValuePair<int, Layer>>();
                                foreach (var layer in Layers)
                                {
                                    if (layer.Key > CurrentLayerIndex)
                                    {
                                        tmpLayers.Add(new KeyValuePair<int, Layer>(layer.Key - 1, layer.Value));
                                    }
                                    else
                                    {
                                        tmpLayers.Add(layer);
                                    }
                                }
                                Layers = tmpLayers;

                                RaisePropertyChanged("Layers");
                                RaisePropertyChanged("CurrentLayer");
                                if (!HasCurrentLayer)
                                {
                                    CurrentLayerIndex--;
                                }
                            }
                        },
                        () => { return HasCurrentLayer && CurrentLayer.IsDeletable; }
                        );
                }
                return _deleteCurrentLayer;
            }
        }

        public ICommand ShowCurrentLayer
        {
            get
            {
                if (_showCurrentLayer == null)
                {
                    _showCurrentLayer = new RelayCommand(
                        () =>
                        {
                            if (HasCurrentLayer)
                            {
                                CurrentLayer.IsDisplayed = true;
                            }
                        },
                        () => { return HasCurrentLayer; }
                        );
                }
                return _showCurrentLayer;
            }
        }

        public ICommand HideCurrentLayer
        {
            get
            {
                if (_hideCurrentLayer == null)
                {
                    _hideCurrentLayer = new RelayCommand(
                        () =>
                        {
                            if (HasCurrentLayer)
                            {
                                CurrentLayer.IsDisplayed = false;
                            }
                        },
                        () => { return HasCurrentLayer && CurrentLayer.IsHideable; }
                        );
                }
                return _hideCurrentLayer;
            }
        }

        public ICommand InitiateRenameCurrentLayer
        {
            get
            {
                if (_initiateRenameCurrentLayer == null)
                {
                    _initiateRenameCurrentLayer = new RelayCommand(
                        () =>
                        {
                            if (HasCurrentLayer)
                            {
                                CurrentLayer.IsRenaming = true;
                            }
                        },
                        () => { return HasCurrentLayer && !CurrentLayer.IsRenaming && CurrentLayer.IsRenameable; }
                        );
                }
                return _initiateRenameCurrentLayer;
            }
        }

        public ICommand ValidateRenameCurrentLayer
        {
            get
            {
                if (_validateRenameCurrentLayer == null)
                {
                    _validateRenameCurrentLayer = new RelayCommand(
                        () =>
                        {
                            if (HasCurrentLayer)
                            {
                                CurrentLayer.IsRenaming = false;
                            }
                        },
                        () => { return HasCurrentLayer && CurrentLayer.IsRenameable; }
                        );
                }
                return _validateRenameCurrentLayer;
            }
        }

        public ICommand ShareCurrentLayer
        {
            get
            {
                if (_shareCurrentLayer == null)
                {
                    _shareCurrentLayer = new RelayCommand(
                        () =>
                        {
                            AppBarState.Close.Execute(null);
                            CurrentLayer.Save();

                            // Opens ShareFlyout
                            var mainwindow = Application.Current.MainWindow as MetroWindow;
                            var flyout = mainwindow.Flyouts.Items[2] as Flyout;
                            var shareLayer = flyout.DataContext as ShareContent;
                            shareLayer.IsLesson = false;
                            shareLayer.GetMembersConnected();
                            flyout.IsOpen = true;
                        },
                        () => { return HasCurrentLayer && CurrentLayer.IsShareable; }
                        );
                }
                return _shareCurrentLayer;
            }
        }

        public ICommand SetDrawingMode
        {
            get
            {
                if (_setDrawingMode == null)
                {
                    _setDrawingMode = new ParametrizedRelayCommand(
                        mode =>
                        {
                            if (mode != null)
                            {
                                var stringMode = (string) mode;
                                if (stringMode == "PointEraser")
                                {
                                    CurrentState.SetInkingMode(LayerStackState.InkingModes.PointEraser);
                                    EraserShape = defaultPointEraserShape;
                                }
                                else if (stringMode == "StrokeEraser")
                                {
                                    CurrentState.SetInkingMode(LayerStackState.InkingModes.StrokeEraser);
                                    EraserShape = defaultStrokeEraserShape;
                                }
                                else if (stringMode == "Selection")
                                {
                                    CurrentState.SetInkingMode(LayerStackState.InkingModes.Selection);
                                }
                                else if (stringMode == "Pencil")
                                {
                                    CurrentState.SetInkingMode(LayerStackState.InkingModes.Pencil);
                                    DrawingAttributes = defaultPencil;
                                }
                                else if (stringMode == "Highlighter")
                                {
                                    CurrentState.SetInkingMode(LayerStackState.InkingModes.Highlighter);
                                    DrawingAttributes = defaultHighlighter;
                                }
                                else if (stringMode == "Pointer")
                                {
                                    CurrentState.SetInkingMode(LayerStackState.InkingModes.Pointer);
                                    DrawingAttributes = defaultPointer;
                                }

                                RadialMenuState.Close();
                                RaisePropertyChanged("CurrentState");
                                RaisePropertyChanged("CurrentDrawingColor");
                            }
                        }
                        );
                }
                return _setDrawingMode;
            }
        }

        public ICommand SetDrawingColor
        {
            get
            {
                if (_setDrawingColor == null)
                {
                    _setDrawingColor = new ParametrizedRelayCommand(
                        color =>
                        {
                            if (color != null)
                            {
                                var stringColor = (string) color;
                                var newColor = DrawingAttributes.Color;
                                if (stringColor == "Black")
                                {
                                    newColor = Colors.Black;
                                }
                                if (stringColor == "Gray")
                                {
                                    newColor = Colors.Gray;
                                }
                                else if (stringColor == "Red")
                                {
                                    newColor = Colors.Red;
                                }
                                else if (stringColor == "Green")
                                {
                                    newColor = Colors.Green;
                                }
                                else if (stringColor == "Blue")
                                {
                                    newColor = Colors.Blue;
                                }
                                else if (stringColor == "Yellow")
                                {
                                    newColor = Colors.Yellow;
                                }
                                DrawingAttributes.Color = newColor;
                                RaisePropertyChanged("CurrentDrawingColor");

                                RadialMenuState.Close();
                            }
                        }
                        );
                }
                return _setDrawingColor;
            }
        }

        public ICommand SetDrawingThickness
        {
            get
            {
                if (_setDrawingThickness == null)
                {
                    _setDrawingThickness = new ParametrizedRelayCommand(
                        thickness =>
                        {
                            if (thickness != null)
                            {
                                var stringThickness = (string) thickness;
                                var newThickness = (int) DrawingAttributes.Width;
                                if (stringThickness == "2")
                                {
                                    newThickness = 2;
                                }
                                else if (stringThickness == "5")
                                {
                                    newThickness = 5;
                                }
                                else if (stringThickness == "10")
                                {
                                    newThickness = 10;
                                }
                                else if (stringThickness == "20")
                                {
                                    newThickness = 20;
                                }
                                else if (stringThickness == "50")
                                {
                                    newThickness = 50;
                                }

                                // Increase Drawing pointer size
                                if (CurrentState.IsDrawing)
                                {
                                    DrawingAttributes.Height = newThickness;
                                    // For Hightlighter the Width is twice thinner than the Heigh
                                    if (CurrentState.InkingMode == LayerStackState.InkingModes.Highlighter)
                                    {
                                        DrawingAttributes.Width = newThickness/2;
                                    }
                                    else
                                    {
                                        DrawingAttributes.Width = newThickness;
                                    }
                                }
                                // Increase Erasing pointer size
                                else if (CurrentState.InkingMode == LayerStackState.InkingModes.PointEraser)
                                {
                                    EraserShape =
                                        defaultPointEraserShape = new RectangleStylusShape(newThickness, newThickness);
                                }

                                RadialMenuState.Close();
                            }
                        }
                        );
                }
                return _setDrawingThickness;
            }
        }

        public ICommand ActivateFingerInking
        {
            get
            {
                if (_activateFingerInking == null)
                {
                    _activateFingerInking = new RelayCommand(
                        () =>
                        {
                            if (CurrentState.State == LayerStackState.States.OnlyDrawing)
                            {
                                CurrentState.SetState(LayerStackState.States.DrawingAndManipulating);
                            }
                            else
                            {
                                CurrentState.SetState(LayerStackState.States.OnlyDrawing);
                            }
                            RadialMenuState.Close();
                            RaisePropertyChanged("CurrentState");
                        }
                        );
                }
                return _activateFingerInking;
            }
        }

        public ICommand SelectLayer
        {
            get
            {
                if (_selectLayer == null)
                {
                    _selectLayer = new ParametrizedRelayCommand(
                        newLayerIndex => { CurrentLayerIndex = (int) newLayerIndex; }
                        );
                }
                return _selectLayer;
            }
        }

        public ICommand ToggleFullscreen
        {
            get
            {
                if (_toggleFullscreen == null)
                {
                    _toggleFullscreen = new RelayCommand(
                        () =>
                        {
                            IsFullscreenMode = !IsFullscreenMode;
                            /*if (IsFullscreenMode)
                            {
                                Viewport.AnimateScrollViewer(-200, 0);
                            }
                            else
                            {
                                Viewport.AnimateScrollViewer(200, 0);
                            }*/
                            RadialMenuState.Close();
                        });
                }
                return _toggleFullscreen;
            }
        }

        public ICommand CloseRadialMenu
        {
            get
            {
                if (_closeRadialMenu == null)
                {
                    _closeRadialMenu = new RelayCommand(
                        () =>
                        {
                            if (RadialMenuState.IsOpen)
                            {
                                RadialMenuState.Close();
                            }
                        }
                        );
                }
                return _closeRadialMenu;
            }
        }

        public ICommand GoToRadialMenu
        {
            get
            {
                if (_goToRadialMenu == null)
                {
                    _goToRadialMenu = new ParametrizedRelayCommand(
                        level =>
                        {
                            if (level != null)
                            {
                                var stringLevel = (string) level;
                                var newLevel = RadialMenuState.Levels.None;
                                switch (stringLevel)
                                {
                                    case "Main":
                                        newLevel = RadialMenuState.Levels.Main;
                                        break;
                                    case "DrawingModes":
                                        newLevel = RadialMenuState.Levels.DrawingModes;
                                        break;
                                    case "Colors":
                                        newLevel = RadialMenuState.Levels.Colors;
                                        break;
                                    case "Thicknesses":
                                        newLevel = RadialMenuState.Levels.Thicknesses;
                                        break;
                                    case "Tags":
                                        newLevel = RadialMenuState.Levels.Tags;
                                        break;
                                }
                                RadialMenuState.Open(newLevel);
                            }
                        }
                        );
                }
                return _goToRadialMenu;
            }
        }

        #endregion

        #region METHODS

        public LayerStack(Holder holderModel, Lessons.Lesson lessonModel)
        {
            this.lessonModel = lessonModel;
            this.holderModel = holderModel;
        }

        internal void Init(Lesson lessonViewModel)
        {
            this.lessonViewModel = lessonViewModel;
            if (holderModel.GetType() == typeof (GlobalNotesHolder))
                SetLayers(0);
            else
                SetLayers();
        }

        public void AddNewAnswerLayer(int offset, QuizAnswerLayer answerLayer)
        {
            var layerViewModel = new Layer(this, lessonModel, answerLayer);
            Layers.Add(new KeyValuePair<int, Layer>(offset, layerViewModel));
            CurrentLayerIndex = Layers.Count - 1; // Select the last Layer (which is the Answer Layer)
            CurrentLayer.DisplayChart(); // Compile Chart for the Layer
        }

        public void AddNewAnswerLayer(int offset, GraphicalAnswerLayer answerLayer)
        {
            var layerViewModel = new Layer(this, lessonModel, answerLayer);
            Layers.Add(new KeyValuePair<int, Layer>(offset, layerViewModel));
            CurrentLayerIndex = Layers.Count - 1; // Select the last Layer (which is the Answer Layer)
            CurrentLayer.DisplaySaliencyMap(); // Compile SaliencyMap for the Layer
        }

        internal void AddNewBullet(Point position)
        {
            var bulletRadius = 15;
            position.Offset(-bulletRadius, -bulletRadius);
            var quiz = ((QuizContent) lessonModel.Exercises.Contents[lessonViewModel.CurrentPageIndex]);
            var bulletPosition = new Layers.Components.Point(position.X, position.Y);
            var bulletIndex = CurrentLayer.GetLastBulletOffset() + 1;
            quiz.AddBullet(bulletIndex, bulletPosition);

            // Forward the adding to each Layer of the View to update it
            foreach (var layer in Layers)
            {
                layer.Value.AddBullet(bulletIndex, position);
            }
        }

        internal void RemoveBullet(QuizBullet quizBullet)
        {
            var quiz = ((QuizContent) lessonModel.Exercises.Contents[lessonViewModel.CurrentPageIndex]);
            quiz.RemoveBullet(quizBullet.Offset);
            CurrentLayer.RemoveBullet(quizBullet);
        }

        internal void RemoveDraggableComponent(DraggableComponent draggableComponent)
        {
            CurrentLayer.RemoveDraggableComponent(draggableComponent);
        }

        internal void AddNewTextBox(Point position)
        {
            var textBoxWidth = 250;
            var textBoxHeight = 150;
            position.Offset(-textBoxWidth/2, -textBoxHeight/2);
            CurrentLayer.AddTextBox(position);
        }

        public void SetLayers(int pageIndex = -1)
        {
            if (pageIndex == -1)
                pageIndex = lessonViewModel.CurrentPageIndex;
            var layers = holderModel.Contents[pageIndex].Layers;
            Layers = new ObservableCollection<KeyValuePair<int, Layer>>();
            for (var i = 0; i < layers.Count; i++)
            {
                Layers.Add(new KeyValuePair<int, Layer>(i, new Layer(this, lessonModel, layers[i])));
            }

            RaisePropertyChanged("Layers");
            RaisePropertyChanged("CurrentLayer");
        }

        private void ForwardCurrentLayersToModel()
        {
            if (HasCurrentLayer)
            {
                CurrentLayer.Save();
            }
        }

        public void Save()
        {
            ForwardCurrentLayersToModel();
        }

        public void OpenRadialMenu(Rect viewport, Point center, RadialMenuState.Levels level)
        {
            if (CurrentState.State != LayerStackState.States.OnlyManipulating)
            {
                // Need to temporarily disable the Drawing (to undo the current drawing)
                var previousState = CurrentState.State;
                CurrentState.SetState(LayerStackState.States.OnlyManipulating);
                RaisePropertyChanged("CurrentState");
                CurrentState.SetState(previousState);
                RaisePropertyChanged("CurrentState");
            }

            var radialMenuRadius = 150;
            center.X = Math.Max(center.X, 0 + radialMenuRadius);
            center.Y = Math.Max(center.Y, 0 + radialMenuRadius);
            center.X = Math.Min(center.X, viewport.Width - radialMenuRadius);
            center.Y = Math.Min(center.Y, viewport.Height - radialMenuRadius);
            RadialMenuState.Open(level, center);
        }

        internal void ContainerTouchDown()
        {
            // Disable Drawing
            if (CurrentState.State != LayerStackState.States.OnlyDrawing)
            {
                CurrentState.SetState(LayerStackState.States.OnlyManipulating);
            }

            RaisePropertyChanged("CurrentState");
        }

        internal void ContainerTouchUp()
        {
            // Re-enable Drawing
            if (CurrentState.State != LayerStackState.States.OnlyDrawing)
            {
                CurrentState.SetState(LayerStackState.States.DrawingAndManipulating);
            }

            RaisePropertyChanged("CurrentState");
        }

        #endregion
    }
}