using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;
using Nine.MVVM;
using Nine.Sharing;
using Nine.Tools;
using Nine.Views.Pages;
using LinkIOcsharp;
using System.Windows.Threading;
using System;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Some methods called from the right-menu to provide share methods
    /// </summary>
    internal class ShareContent : BaseViewModel
    {
        private ICommand _closeShareFlyout;
        private ObservableCollection<StateMember> _members;
        private ICommand _sendContent;
        private ICommand _sendContentAll;

        public ObservableCollection<StateMember> Members
        {
            get
            {
                if (_members == null)
                    Members = new ObservableCollection<StateMember>();
                return _members;
            }
            set
            {
                _members = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SendContent
        {
            get
            {
                if (_sendContent == null)
                    _sendContent = new RelayCommand(() => Send());
                return _sendContent;
            }
            private set
            {
                _sendContent = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SendContentAll
        {
            get
            {
                if (_sendContentAll == null)
                    _sendContentAll = new RelayCommand(() => SendAll());
                return _sendContentAll;
            }
            private set
            {
                _sendContentAll = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CloseShareFlyout
        {
            get
            {
                if (_closeShareFlyout == null)
                    _closeShareFlyout = new RelayCommand(() => Close());
                return _closeShareFlyout;
            }
            private set
            {
                _closeShareFlyout = value;
                RaisePropertyChanged();
            }
        }

        internal bool IsLesson { get; set; }

        private void Send()
        {
            Close();

            if (Catalog.Instance.CurrentPageName == "HomePage")
                SendLessonHomePage();
            if (Catalog.Instance.CurrentPageName == "MainPage")
            {
                if (IsLesson)
                {
                    SendLessonMainPage();
                    IsLesson = false;
                }
                else
                    SendSlideLayer();
            }
            if (Catalog.Instance.CurrentPageName == "FreeNotesPage")
                SendFreeNotesLayer();
        }

        private void SendAll()
        {
            foreach (var member in Members)
            {
                member.IsChecked = true;
            }

            Send();
        }

        private void SendLessonHomePage()
        {
            var lessonStack = Catalog.Instance.GetPage("HomePage").DataContext as LessonStack;
            var lessonName = lessonStack.currentLesson.Name;
            var pdf = File.ReadAllBytes(Data.GetPdfFile(lessonName));
            var holderName = Data.Instance.User.CompleteName;
            var sharedLesson = new SharedLesson(lessonName, pdf, holderName);

            foreach (var member in Members)
            {
                if (member.Name != Data.Instance.User.CompleteName && member.IsChecked)
                    LinkIOImp.Instance.send("lesson", sharedLesson, member.ID);
            }
        }

        private void SendLessonMainPage()
        {
            var lessonName = Data.Instance.Lesson.Name;
            var pdf = File.ReadAllBytes(Data.GetPdfFile(lessonName));
            var holderName = Data.Instance.User.CompleteName;
            var sharedLesson = new SharedLesson(lessonName, pdf, holderName);

            foreach (var member in Members)
            {
                if (member.Name != Data.Instance.User.CompleteName && member.IsChecked)
                    LinkIOImp.Instance.send("lesson", sharedLesson, member.ID);
            }
        }

        private void SendSlideLayer()
        {
            var MainPage = (MainPage) Catalog.Instance.GetPage("MainPage");
            var layerStack = (MainPage.LayerStack.DataContext as LayerStack);

            var sharedLayer = new SharedParallelLayer(
                (ParallelHolder) layerStack.holderModel,
                layerStack.holderModel.Contents[layerStack.lessonViewModel.CurrentPageIndex].Layers[
                    layerStack.CurrentLayerIndex].UID,
                Data.Instance.User.CompleteName,
                Data.Instance.User.IsTeacher
                );


            foreach (var member in Members)
            {
                if (member.Name != Data.Instance.User.CompleteName && member.IsChecked)
                    LinkIOImp.Instance.send("slide-layer", sharedLayer, member.ID);
            }
        }

        private void SendFreeNotesLayer()
        {
            var FreeNotes = (FreeNotesPage) Catalog.Instance.GetPage("FreeNotesPage");
            var layerStack = FreeNotes.LayerStackDC;
            var sharedLayer = new SharedParallelLayer(
                (ParallelHolder) layerStack.holderModel,
                layerStack.holderModel.Contents[layerStack.lessonViewModel.CurrentPageIndex].Layers[
                    layerStack.CurrentLayerIndex].UID,
                Data.Instance.User.CompleteName,
                Data.Instance.User.IsTeacher
                );


            foreach (var member in Members)
            {
                if (member.Name != Data.Instance.User.CompleteName && member.IsChecked)
                    LinkIOImp.Instance.send("freenotes-layer", sharedLayer, member.ID);
            }
        }

        private void SendExerciseLayer()
        {
            var Questions = (QuestionsPage) Catalog.Instance.GetPage("QuestionsPage");
            var layerStack = Questions.LayerStackDC;

            var content =
                Data.Instance.Lesson.Exercises.Contents[layerStack.LessonViewModel.CurrentPageIndex] as ExerciseContent;

            var sharedLayer = new SharedExerciseLayer(
                content,
                content.Layers[layerStack.CurrentLayerIndex].UID,
                Data.Instance.User.CompleteName,
                Data.Instance.User.IsTeacher
                );


            foreach (var member in Members)
            {
                if (member.Name != Data.Instance.User.CompleteName && member.IsChecked)
                    LinkIOImp.Instance.send("question-layer", sharedLayer, member.ID);
            }
        }

        private void Close()
        {
            var mainwindow = Application.Current.MainWindow as MetroWindow;
            var flyout = mainwindow.Flyouts.Items[2] as Flyout;
            flyout.IsOpen = false;
        }

        internal void GetMembersConnected()
        {
            Members.Clear();
            LinkIOImp.Instance.getAllUsersInCurrentRoom((users) => {
                Nine.App.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var member in users)
                        if (member.Login != Data.Instance.User.Login)
                            Members.Add(new StateMember(member.ID, member.Login));
                    RaisePropertyChanged("Members");
                });
            });
        }
    }


    public class StateMember : BaseViewModel
    {
        public StateMember(string id, string name)
        {
            ID = id;
            Name = name;
            IsChecked = false;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            RaisePropertyChanged("Members");
        }
    }
}