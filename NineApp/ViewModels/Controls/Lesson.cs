using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Nine.MVVM;
using Nine.Roles;
using Nine.Sharing;
using Nine.Tools;
using Nine.Views.Pages;
using LinkIOcsharp;
using LinkIOcsharp.model;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Lesson is the datacontext of the 1st (& indirectly the 2nd) mode (slides & free-notes)
    /// </summary>
    public class Lesson : BaseViewModel
    {
        private BitmapSource _currentPage;
        private int _currentPageIndex;
        private ICommand _goToPage;

        /// <summary>
        ///     Property about the "synchronize" button on left bottom (SideBar)
        ///     Permit to determine some behavior on reception/notification
        /// </summary>
        private bool? _isSync = true;

        private LayerStack _layerStack;
        private int _teacherPageIndex = -1;
        private ObservableCollection<KeyValuePair<int, BitmapSource>> _thumbs;
        protected Views.Controls.Lesson lessonPageComponent;
        protected ItemsControl thumbsContainerComponent;

        public Lesson()
        {
            // Create child DataContext by passing initilized model
            LayerStack = new LayerStack(Data.Instance.Lesson.Slides, Data.Instance.Lesson);
            LayerStack.Init(this);

            // Render the Bitmap for current page and thumbs
            Render();

            // As teacher, when i'm joining a lesson, I'm sharing my current position
            if (Data.Instance.Role == Role.TEACHER)
                SendTeacherPosition();
            else // As student, i'm asking what is the position of the teacher on his slide
                LinkIOImp.Instance.send("slide-index", new AskingSlide(0), new List<User>() { User.getTestUsers()[0]});
        }

        public string LessonName
        {
            get { return Data.Instance.Lesson.Name; }
        }

        public bool? IsSync
        {
            get { return _isSync; }
            set
            {
                if (_isSync.Value == false && value.Value && Data.Instance.Role == Role.STUDENT)
                    ChangeCurrentPage(TeacherPageIndex);
                _isSync = value;
                RaisePropertyChanged();
            }
        }

        public BitmapSource CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                if (value != _currentPageIndex && value >= 0 && value < PageCount)
                {
                    _currentPageIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        public virtual int PageCount
        {
            get { return Data.Instance.Lesson.Slides.Contents.Count; }
        }

        public ObservableCollection<KeyValuePair<int, BitmapSource>> Thumbs
        {
            get
            {
                if (_thumbs == null)
                {
                    Thumbs = new ObservableCollection<KeyValuePair<int, BitmapSource>>();
                }
                return _thumbs;
            }
            private set
            {
                _thumbs = value;
                RaisePropertyChanged();
            }
        }

        public LayerStack LayerStack
        {
            get { return _layerStack; }
            set
            {
                _layerStack = value;
                RaisePropertyChanged();
            }
        }

        public LayerStack LayerStackFreeNotes
        {
            get { return (Catalog.Instance.GetPage("FreeNotesPage").DataContext as FreeNotesPage).LayerStackDC; }
        }

        private LayerStack CurrentLayerStack
        {
            get
            {
                if (Catalog.Instance.IsMainPage)
                    return LayerStack;
                return LayerStackFreeNotes;
            }
        }

        public ICommand GoToPage
        {
            get
            {
                if (_goToPage == null)
                {
                    _goToPage = new ParametrizedRelayCommand(page => GoingToPage((int) page));
                }
                return _goToPage;
            }
            private set { _goToPage = value; }
        }

        public int TeacherPageIndex
        {
            get { return _teacherPageIndex; }
            set
            {
                if (value < 0 || value >= PageCount)
                {
                    return;
                }
                _teacherPageIndex = value;
                RaisePropertyChanged();
            }
        }

        public virtual bool CanCollect
        {
            get { return false; }
        } // Override in ViewModels/Controls/Exercices.cs

        public virtual bool CanAnswer
        {
            get { return false; }
        } // Override in ViewModels/Controls/Exercices.cs

        public virtual bool CanBeStart
        {
            get { return true; }
        } // Override in ViewModels/Controls/Exercices.cs

        public virtual ICommand SubmitAnswer
        {
            get
            {
                return new RelayCommand(
                    () =>
                    {
                        // Override in ViewModels/Controls/Exercices.cs
                    });
            }
        }

        public virtual void GoingToPage(int page)
        {
            if (Data.Instance.Role == Role.STUDENT)
                IsSync = false;

            ChangeCurrentPage(page);
        }

        internal virtual void SendTeacherPosition()
        {
            LinkIOImp.Instance.send("new-page-index", CurrentPageIndex);
        }

        internal void SetComponents(Views.Controls.Lesson LessonPage, ItemsControl thumbsContainer)
        {
            lessonPageComponent = LessonPage;
            thumbsContainerComponent = thumbsContainer;
        }

        private void GoToPreviousPage()
        {
            ChangeCurrentPage(CurrentPageIndex - 1);
        }

        private void GoToNextPage()
        {
            ChangeCurrentPage(CurrentPageIndex + 1);
        }

        private void GoToFirstPage()
        {
            ChangeCurrentPage(0);
        }

        private void GoToLastPage()
        {
            ChangeCurrentPage(PageCount - 1);
        }

        internal virtual void ChangeCurrentPage(int newPageIndex)
        {
            if (CurrentPageIndex != newPageIndex && newPageIndex >= 0 && newPageIndex < PageCount)
            {
                CurrentPageIndex = newPageIndex;
                RenderCurrentPage();

                // Scrolls to current thumb inside the SideBar
                var currentThumb =
                    thumbsContainerComponent.ItemContainerGenerator.ContainerFromIndex(CurrentPageIndex) as
                        ContentPresenter;
                currentThumb.BringIntoView();

                // Only teacher share is current position and ONLY if it is sync
                if (Data.Instance.Role == Role.TEACHER && IsSync.Value)
                    LinkIOImp.Instance.send("new-page-index", newPageIndex);
            }

            // If we're not on the MainPage (click on thumb when we are in FreeNotePage)
            if (!Catalog.Instance.IsMainPage && Catalog.Instance.Exists("MainPage"))
                Catalog.Instance.NavigateTo("MainPage");
        }

        private void Render()
        {
            RenderCurrentPage();
            RenderThumbs();
        }

        private void RenderCurrentPage()
        {
            // Get Layers of the current page from Lesson model
            // to expose them to the view model of the LayerStack
            LayerStack.SetLayers();

            var currentPagePath = Data.GetPageFile(Data.Instance.Lesson.Name, CurrentPageIndex);
            CurrentPage = BitmapHelper.Load(currentPagePath);
        }

        private void RenderThumbs()
        {
            for (var i = 0; i < PageCount; i++)
            {
                var thumbPath = Data.GetThumbFile(Data.Instance.Lesson.Name, i);
                var thumb = BitmapHelper.Load(thumbPath);
                Thumbs.Add(new KeyValuePair<int, BitmapSource>(i, thumb));
            }
        }

        internal void ContainerTouchUp()
        {
            // If a zoom is performered -> re-render CurrentPage with corresponding scale
            //RenderCurrentPage();
        }

        public void AppKeyDown(KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Catalog.Instance.CurrentPageName == "MainPage" ||
                    Catalog.Instance.CurrentPageName == "FreeNotesPage")
                {
                    var switchSucceed = true;

                    switch (e.Key)
                    {
                        case Key.Left:
                        case Key.Up:
                            GoToPreviousPage();
                            break;
                        case Key.Right:
                        case Key.Down:
                            GoToNextPage();
                            break;
                        case Key.Home:
                        case Key.PageUp:
                            GoToFirstPage();
                            break;
                        case Key.End:
                        case Key.PageDown:
                            GoToLastPage();
                            break;
                        default:
                            switchSucceed = false;
                            break;
                    }

                    if (switchSucceed)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        internal void CenterScrollViewer()
        {
            LayerStack.Viewport.CenterScrollViewer(lessonPageComponent);
        }
    }
}