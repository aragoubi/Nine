using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Nine.MVVM;
using Nine.Tools;
using Nine.ViewModels.Windows;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// The LessonStack is the DataContext of the component on the MainPage that allow to show existing lessons.
    /// </summary>
    internal class LessonStack : BaseViewModel
    {
        private ICommand _addPDF;
        private string _directoryPDF;
        private int _indexCurrentLesson;
        private bool _isBusy;
        private bool _isOpen;
        private string _lessonName;
        private ObservableCollection<KeyValuePair<int, MinimalLesson>> _lessons;
        private ICommand _loadLesson;
        private ICommand _lostFocus;
        private string _oldLessonName;
        private ICommand _openFlyout;
        private ICommand _selectLesson;
        private bool _staysOpen;
        private ObservableCollection<KeyValuePair<int, BitmapSource>> _thumbs;
        private ICommand _validateRenaming;

        public int IndexCurrentLesson
        {
            get { return _indexCurrentLesson; }
            set
            {
                _indexCurrentLesson = value;
                RaisePropertyChanged();
            }
        }

        public MinimalLesson currentLesson
        {
            get { return Lessons[IndexCurrentLesson].Value; }
        }

        public ObservableCollection<KeyValuePair<int, MinimalLesson>> Lessons
        {
            get
            {
                if (_lessons == null)
                {
                    _lessons = new ObservableCollection<KeyValuePair<int, MinimalLesson>>();
                }
                return _lessons;
            }
            set
            {
                _lessons = value;
                RaisePropertyChanged();
            }
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

        public string LessonName
        {
            get { return _lessonName; }
            set
            {
                _lessonName = value;
                RaisePropertyChanged();
            }
        }

        public string DirectoryPDF
        {
            get { return _directoryPDF; }
            set
            {
                _directoryPDF = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadPDF
        {
            get
            {
                if (_addPDF == null)
                    _addPDF = new RelayCommand(() => ReachFile());
                return _addPDF;
            }

            private set
            {
                _addPDF = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadLesson
        {
            get
            {
                if (_loadLesson == null)
                    _loadLesson = new RelayCommand(() => AddLesson());
                return _loadLesson;
            }
            private set
            {
                _loadLesson = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public bool StaysOpen
        {
            get { return _staysOpen; }
            set
            {
                if (_staysOpen == value) return;
                _staysOpen = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value) return;
                _isOpen = value;

                if (_isOpen == false)
                {
                    LessonName = null;
                    DirectoryPDF = null;
                }

                RaisePropertyChanged();
            }
        }

        public ICommand SelectLesson
        {
            get
            {
                if (_selectLesson == null)
                    _selectLesson = new ParametrizedRelayCommand(lesson => OpenLesson((int) lesson));
                return _selectLesson;
            }
            private set
            {
                _selectLesson = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenAppBar
        {
            get
            {
                if (_openFlyout == null)
                    _openFlyout = new ParametrizedRelayCommand(lesson => OpenFlyout((int) lesson));
                return _openFlyout;
            }
            private set
            {
                _openFlyout = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LostFocus
        {
            get
            {
                if (_lostFocus == null)
                {
                    _lostFocus = new RelayCommand(() => RenameLessonFalse());
                }
                return _lostFocus;
            }
        }

        public ICommand ValidateRenaming
        {
            get
            {
                if (_validateRenaming == null)
                {
                    _validateRenaming = new RelayCommand(() => RenameLesson());
                }
                return _validateRenaming;
            }
        }

        public string WelcomeMessage
        {
            get { return "Bonjour " + Data.Instance.User.FirstName + " !"; }
        }

        private void CloseAllPage()
        {
            Catalog.Instance.RemovePage("MainPage");
            Catalog.Instance.RemovePage("FreeNotesPage");
            Catalog.Instance.RemovePage("QuestionsPage");
        }

        internal void ExistingLessons(object sender, RoutedEventArgs e)
        {
            // Reset previous loaded lesson
            Lessons = null;
            CloseAllPage();


            var lessonFolders = Directory.GetDirectories(Data.NineFolder);
            foreach (var lessonFolder in lessonFolders)
            {
                var lessonName = Path.GetFileName(lessonFolder);
                var dataFile = Data.GetDataFile(lessonName);
                var pdfPath = Data.GetPdfFile(lessonName);
                var thumbPath = Data.GetPageFile(lessonName, 0);

                if (File.Exists(dataFile) && File.Exists(pdfPath) && File.Exists(thumbPath))
                {
                    var bitmap = BitmapHelper.Load(thumbPath);
                    Lessons.Add(
                        new KeyValuePair<int, MinimalLesson>(
                            Lessons.Count(),
                            new MinimalLesson(lessonName, bitmap, false)
                            )
                        );
                }
                else
                {
                    try
                    {
                        Directory.Delete(lessonFolder, true);
                    }
                    catch (IOException e1)
                    {
                        MessageBox.Show(e1.Message);
                    }
                }
            }
        }

        private void ReachFile()
        {
            var dialogBox = new OpenFileDialog();
            dialogBox.InitialDirectory = Data.NineFolder;
            dialogBox.Filter = "PDF (*.pdf)|*.pdf";
            dialogBox.Multiselect = false;
            dialogBox.AddExtension = true;
            dialogBox.CheckFileExists = true;

            StaysOpen = true;

            var result = dialogBox.ShowDialog();
            if (result == true)
            {
                DirectoryPDF = dialogBox.FileName;
            }
        }

        private async void AddLesson()
        {
            if (LessonName != null && DirectoryPDF != null)
            {
                var nineManagement = Application.Current.MainWindow.DataContext as NineManagement;

                await nineManagement.ShowProgressDialogAddLesson(
                    "Veuillez patienter...",
                    "   Votre leçon est en cours de création !",
                    LessonName,
                    DirectoryPDF
                    );

                LessonName = null;
                DirectoryPDF = null;
            }
        }

        private void OpenLesson(int nbLesson)
        {
            Lessons.Lesson lesson;
            var lessonFile = Data.GetDataFile(Lessons[nbLesson].Value.Name);
            var serializedLesson = new FileStream(lessonFile, FileMode.Open);
            try
            {
                var formatter = new BinaryFormatter();

                // Deserialize the lesson from the file and 
                // assign the reference to the local variable.
                lesson = (Lessons.Lesson) formatter.Deserialize(serializedLesson);
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Leçon impossible à désérialiser (vérifiez la compatibilité des versions)");
                Console.WriteLine(e.Message);
                return;
            }
            finally
            {
                serializedLesson.Close();
            }

            Data.Instance.Lesson = lesson;
            Catalog.Instance.ReInit("MainPage");
            Catalog.Instance.ReInit("FreeNotesPage");
            Catalog.Instance.ReInit("QuestionsPage");
            Catalog.Instance.NavigateTo("MainPage");
        }

        public void OpenFlyout(int lesson)
        {
            IndexCurrentLesson = lesson;

            var mainwindow = Application.Current.MainWindow as MetroWindow;
            var flyout = mainwindow.Flyouts.Items[1] as Flyout;
            if (flyout.IsOpen != true)
                flyout.IsOpen = !flyout.IsOpen;
        }

        public void DeleteLesson()
        {
            if (_lessons != null && _lessons.Count > 0 && IndexCurrentLesson >= 0 && IndexCurrentLesson < _lessons.Count)
            {
                var selectedLesson = Lessons[IndexCurrentLesson].Value.Name;

                // Removes from lesson collection
                Lessons.RemoveAt(IndexCurrentLesson);

                var tmpLessons = new ObservableCollection<KeyValuePair<int, MinimalLesson>>();
                foreach (var lesson in Lessons)
                {
                    if (lesson.Key > IndexCurrentLesson)
                    {
                        tmpLessons.Add(new KeyValuePair<int, MinimalLesson>(lesson.Key - 1, lesson.Value));
                    }
                    else
                    {
                        tmpLessons.Add(lesson);
                    }
                }
                Lessons = tmpLessons;
                RaisePropertyChanged("Lessons");

                // Remove from HDD
                var directory = Data.GetLessonFolder(selectedLesson);
                if (Directory.Exists(directory))
                {
                    try
                    {
                        Directory.Delete(directory, true);
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }

                RaisePropertyChanged("Lessons");
            }
        }

        public void RenameLessonTrue()
        {
            Lessons[IndexCurrentLesson].Value.IsRenaming = true;
            foreach (var lesson in Lessons)
            {
                if (lesson.Key != IndexCurrentLesson)
                    lesson.Value.IsRenaming = false;
            }

            _oldLessonName = Lessons[IndexCurrentLesson].Value.Name;
        }

        public void RenameLessonFalse()
        {
            foreach (var lesson in Lessons)
            {
                lesson.Value.IsRenaming = false;
            }
        }

        public void RenameLesson()
        {
            RenameLessonFalse();

            var newNameLesson = Lessons[IndexCurrentLesson].Value.Name;

            //Rename directory
            var newDirectory = Data.GetLessonFolder(newNameLesson);
            var oldDirectory = Data.GetLessonFolder(_oldLessonName);

            if (Directory.Exists(oldDirectory) && oldDirectory != newDirectory)
            {
                try
                {
                    Directory.Move(oldDirectory, newDirectory);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                }

                Lessons.Lesson lesson;
                var lessonFile = Data.GetDataFile(currentLesson.Name);
                var serializedLesson = new FileStream(lessonFile, FileMode.Open);
                try
                {
                    var formatter = new BinaryFormatter();

                    // Deserialize the lesson from the file and 
                    // assign the reference to the local variable.
                    lesson = (Lessons.Lesson) formatter.Deserialize(serializedLesson);
                }
                catch (SerializationException e)
                {
                    MessageBox.Show("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    serializedLesson.Close();
                }

                lesson.Name = newNameLesson;

                var saveDirectory = Path.Combine(newDirectory, "data.nine");
                var fs = new FileStream(saveDirectory, FileMode.Create);

                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fs, lesson);
                }
                catch (SerializationException e)
                {
                    MessageBox.Show("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }
    }

    #region classe MinimalLesson

    public class MinimalLesson : BaseViewModel
    {
        private bool _isRenaming;
        private string _name;

        public MinimalLesson(string first, BitmapSource second, bool triple)
        {
            Name = first;
            Thumb = second;
            IsRenaming = triple;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public BitmapSource Thumb { get; set; }

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
    }

    #endregion
}