using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.Lessons.Contents.Processing;
using Nine.MVVM;
using Nine.Roles;
using Nine.Sharing;
using Nine.Tools;
using LinkIOcsharp;
using LinkIOcsharp.model;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Proper datacontet (indirect) for the 3rd mode (exercises) of the app, extends of Lesson to rewrite some behaviors & requirements.
    /// </summary>
    public class Exercises : Lesson
    {
        private ICommand _beginToComposeExercise;
        private ICommand _collectCurrentExercise;
        private ICommand _createExercise;
        private string _exerciseName;
        private ICommand _openNewExcercisePopup;
        private bool _popupIsOpen;
        private ObservableCollection<KeyValuePair<int, DescEx>> _questions;
        private ICommand _startCurrentExercise;
        private ICommand _submitAnswer;

        public Exercises(LayerStack layerStack, int width, int height)
        {
            // Create child DataContext by passing initilized model
            LayerStack = layerStack;

            Width = width;
            Height = height;
        }

        public override int PageCount
        {
            get { return Data.Instance.Lesson.Exercises.Contents.Count; }
        }

        public ICommand ClosePopup
        {
            get
            {
                return new RelayCommand(
                    () =>
                    {
                        PopupIsOpen = false;
                        ExerciseName = "";
                    });
            }
        }

        public ICommand CreateExercise
        {
            get
            {
                if (_createExercise == null)
                {
                    _createExercise = new RelayCommand(
                        () => { PopupIsOpen = true; });
                }
                return _createExercise;
            }
        }

        public ICommand BeginToComposeExercise
        {
            get
            {
                if (_beginToComposeExercise == null)
                {
                    _beginToComposeExercise = new ParametrizedRelayCommand(
                        param =>
                        {
                            PopupIsOpen = false;
                            var _exFactory = new ExerciseContent.ExerciseFactory(Data.Instance.Lesson.Exercises);
                            ExerciseContent ex = null;
                            var type = (string) param;
                            switch (type)
                            {
                                case "QCM":
                                    ex = _exFactory.GetQCM(ExerciseName);
                                    (ex as QuizContent).AddQuestionLayer("Question");
                                    (ex as QuizContent).AddAnswerLayer("Réponse");
                                    break;
                                case "QCU":
                                    ex = _exFactory.GetQCU(ExerciseName);
                                    (ex as QuizContent).AddQuestionLayer("Question");
                                    (ex as QuizContent).AddAnswerLayer("Réponse");
                                    break;
                                case "Graphical":
                                    ex = _exFactory.GetGraphical(ExerciseName);
                                    ex.AddQuestionLayer("Question");
                                    ex.AddAnswerLayer("Réponse");
                                    break;
                                default:
                                    break;
                            }
                            Data.Instance.Lesson.Exercises.Contents.Add(ex);
                            Questions.Add(
                                new KeyValuePair<int, DescEx>(
                                    Questions.Count,
                                    new DescEx(
                                        ExerciseName,
                                        (Data.Instance.Lesson.Exercises.Contents[
                                            Data.Instance.Lesson.Exercises.Contents.Count - 1] as ExerciseContent).Kind)));
                            RaisePropertyChanged();
                            ChangeCurrentPage();
                            RaisePropertyChanged("CanBeStart");
                        });
                }
                ExerciseName = "";
                return _beginToComposeExercise;
            }
        }

        public ICommand StartCurrentExercise
        {
            get
            {
                if (_startCurrentExercise == null)
                    _startCurrentExercise = new RelayCommand(
                        () =>
                        {
                            LayerStack.AppBarState.Close.Execute(null);
                            LayerStack.CurrentLayer.Save();

                            (Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent).Accept = true;
                            RaisePropertyChanged("CanCollect");

                            var exercise = Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent;
                            var layer = exercise.Layers[LayerStack.CurrentLayerIndex];


                            if (layer.GetType() == typeof (QuizLayer) ||
                                layer.GetType().IsSubclassOf(typeof (QuizLayer)))
                                LinkIOImp.Instance.send("quiz-ask",
                                    new SharedQuizExercise(layer as QuizLayer, exercise.UID,
                                        (exercise as QuizContent).Bullets, (exercise as QuizContent).Mode, exercise.Name,
                                        Data.Instance.User.CompleteName));
                            else
                            {
                                var proc = exercise.Processing as GraphicalProcessing;
                                LinkIOImp.Instance.send("graphical-ask",
                                    new SharedGraphicExercise(layer as BasicLayer, exercise.UID, exercise.Name,
                                        Data.Instance.User.CompleteName, proc.Width, proc.Height));
                            }
                        });
                return _startCurrentExercise;
            }
        }

        public ICommand CollectCurrentExercise
        {
            get
            {
                if (_collectCurrentExercise == null)
                    _collectCurrentExercise = new RelayCommand(
                        () =>
                        {
                            var exercise =
                                (Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent);

                            exercise.Accept = false;
                            exercise.HasBeenCollected = true;

                            RaisePropertyChanged("CanCollect");
                            RaisePropertyChanged("CanBeStart");

                            var answerLayerViewModel = LayerStack.Layers[ExerciseContent.AnswerLayerIndex].Value;

                            if (exercise.GetType() == typeof (QuizContent))
                            {
                                var quiz = exercise as QuizContent;
                                if (quiz != null)
                                {
                                    quiz.ComputeResults(); // Updates model with new Data
                                    answerLayerViewModel.DisplayChart(); // Update view
                                }
                            }
                            else
                            {
                                var graphic = exercise;
                                if (graphic != null)
                                {
                                    graphic.ComputeResults(); // Updates model with new Data
                                    answerLayerViewModel.DisplaySaliencyMap(); // Update view
                                }
                            }

                            // Refresh view
                            LayerStack.SetLayers();
                        });
                return _collectCurrentExercise;
            }
        }

        public override ICommand SubmitAnswer
        {
            get
            {
                if (_submitAnswer == null)
                    _submitAnswer = new RelayCommand(
                        () =>
                        {
                            var exercise = Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent;
                            var layer = exercise.Layers[LayerStack.CurrentLayerIndex];
                            if (layer.GetType() == typeof (QuizLayer) ||
                                layer.GetType().IsSubclassOf(typeof (QuizLayer)))
                                LinkIOImp.Instance.send("quiz-answer",
                                    new AnswerLayer<QuizLayer>(exercise.UID, layer as QuizLayer),
                                    new List<User>() { User.getTestUsers()[0] }
                                    );
                            else
                            {
                                LayerStack.Save();
                                LinkIOImp.Instance.send("graphical-answer",
                                    new AnswerLayer<BasicLayer>(exercise.UID, layer as BasicLayer),
                                    new List<User>() { User.getTestUsers()[0] }
                                    );
                            }
                            exercise.Answered = true;
                            RaisePropertyChanged("CanAnswer");
                        });
                return _submitAnswer;
            }
        }

        public bool IsTeacherMode
        {
            get { return Data.Instance.Role == Role.TEACHER; }
        }

        public ObservableCollection<KeyValuePair<int, DescEx>> Questions
        {
            get
            {
                if (_questions == null)
                {
                    var lessonModel = Data.Instance.Lesson;
                    _questions = new ObservableCollection<KeyValuePair<int, DescEx>>();
                    var i = 0;
                    foreach (ExerciseContent exercise in lessonModel.Exercises.Contents)
                    {
                        Questions.Add(new KeyValuePair<int, DescEx>(i, new DescEx(exercise.Name, exercise.Kind)));
                        i++;
                    }
                }
                return _questions;
            }
            private set
            {
                _questions = value;
                RaisePropertyChanged();
            }
        }

        public override bool CanAnswer
        {
            get
            {
                return (!Data.Instance.User.IsTeacher
                        && !(Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent).Answered);
            }
        }

        public override bool CanCollect
        {
            get
            {
                return (
                    Catalog.Instance.CurrentPageName == "QuestionsPage"
                    && Data.Instance.User.IsTeacher
                    && (Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent).Accept);
            }
        }

        public override bool CanBeStart
        {
            get
            {
                return (
                    Catalog.Instance.CurrentPageName == "QuestionsPage"
                    && Data.Instance.User.IsTeacher
                    && !(Data.Instance.Lesson.Exercises.Contents[CurrentPageIndex] as ExerciseContent).HasBeenCollected);
            }
        }

        public ICommand OpenNewExcercisePopup
        {
            get
            {
                if (_openNewExcercisePopup == null)
                {
                    _openNewExcercisePopup = new RelayCommand(
                        () => { PopupIsOpen = true; }
                        );
                }
                return _openNewExcercisePopup;
            }
        }

        public bool PopupIsOpen
        {
            get { return _popupIsOpen; }
            set
            {
                _popupIsOpen = value;
                RaisePropertyChanged();
            }
        }

        public string ExerciseName
        {
            get { return _exerciseName; }
            set
            {
                _exerciseName = value;
                RaisePropertyChanged();
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public override void GoingToPage(int page)
        {
            ChangeCurrentPage(page);
            RaisePropertyChanged("CanCollect");
        }

        internal override void ChangeCurrentPage(int newPageIndex = -1)
        {
            if (newPageIndex == -1)
                newPageIndex = LayerStack.holderModel.Contents.Count - 1;

            if (CurrentPageIndex == newPageIndex || newPageIndex < 0 || newPageIndex >= PageCount)
                return;

            if (Data.Instance.Lesson.Exercises.Contents[newPageIndex].Layers.Count > 1
                && Data.Instance.Lesson.Exercises.Contents[newPageIndex].GetType() == typeof (ExerciseContent)
                && Data.Instance.Lesson.Exercises.Contents[newPageIndex].GetType() != typeof (QuizContent)
                && !Data.Instance.User.IsTeacher)
                LayerStack.CurrentLayerIndex = 1;
            else
                LayerStack.CurrentLayerIndex = 0;

            CurrentPageIndex = newPageIndex;
            LayerStack.SetLayers();

            // Scrolls to current thumb inside the SideBar
            var currentThumb =
                thumbsContainerComponent.ItemContainerGenerator.ContainerFromIndex(CurrentPageIndex) as ContentPresenter;

            if (currentThumb == null)
                return;

            currentThumb.BringIntoView();

            RaisePropertyChanged("CanCollect");
            RaisePropertyChanged("CanAnswer");
            RaisePropertyChanged("CanBeStart");
        }
    }

    public class DescEx
    {
        public DescEx(string name, string kind)
        {
            Name = name;
            Kind = kind;
        }

        public string Name { get; set; }
        public string Kind { get; set; }
    }
}