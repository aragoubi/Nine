using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MuPdf;
using Nine.MVVM;
using Nine.Tools;
using Nine.ViewModels.Controls;
using Nine.Views.Pages;
using Nine.Views.Windows;
using Lesson = Nine.Lessons.Lesson;

namespace Nine.ViewModels.Windows
{
    /// <summary>
    /// DataContext of the Window for the low-level gesture of the lesson (save / rename / create / open).
    /// </summary>
    public class NineManagement : BaseViewModel
    {
        public NineManagement()
        {
            var yevents = new YEvents(this);
            yevents.Register();
        }

        #region MainSideBar

        public void CloseFlyout()
        {
            var mainWindow = (Application.Current.MainWindow as MetroWindow);
            foreach (Flyout flyout in mainWindow.Flyouts.Items)
            {
                if (flyout.IsOpen)
                    flyout.IsOpen = false;
            }
        }

        private ICommand _saveFile;

        public ICommand SaveFile
        {
            get
            {
                if (_saveFile == null)
                    _saveFile = new RelayCommand(() => Save());
                return _saveFile;
            }
        }

        public void Save()
        {
            CloseFlyout();

            if (Data.Instance.Lesson != null)
            {
                // Calls CurrentLayer.Save();
                var MainPage = (MainPage) Catalog.Instance.GetPage("MainPage");
                (MainPage.LayerStack.DataContext as LayerStack).Save();

                var FreeNotesPage = (FreeNotesPage) Catalog.Instance.GetPage("FreeNotesPage");
                FreeNotesPage.LayerStackDC.Save();

                var QuestionsPage = (QuestionsPage) Catalog.Instance.GetPage("QuestionsPage");
                QuestionsPage.LayerStackDC.Save();

                var DefaultSaveDirectory = Path.Combine(Data.NineFolder, Data.Instance.Lesson.Name, "data.nine");
                var fs = new FileStream(DefaultSaveDirectory, FileMode.Create);

                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                var formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, Data.Instance.Lesson);
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

        internal async Task ShowProgressDialogAddLesson(string title, string message, string lessonName,
            string directoryPDF)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);

            var controller = await mainWindow.ShowProgressAsync(title, message);
            controller.SetCancelable(true);

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            await Task.Run(async () => //Task.Run automatically unwraps nested Task types!
            {
                // Control the name
                var baseName = lessonName;
                var i = 1;
                while (Directory.Exists(Data.GetLessonFolder(lessonName)))
                    lessonName = baseName + " " + i++;

                if (Path.GetExtension(Path.GetExtension(directoryPDF)) == "")
                    directoryPDF += ".pdf";

                // Initial model data
                var lessonFolder = Data.GetLessonFolder(lessonName);
                var thumbsFolder = Data.GetThumbsFolder(lessonName);
                var pagesFolder = Data.GetPagesFolder(lessonName);
                var pdfPath = Data.GetPdfFile(lessonName);

                Directory.CreateDirectory(lessonFolder);
                Directory.CreateDirectory(thumbsFolder);
                Directory.CreateDirectory(pagesFolder);
                File.Copy(directoryPDF, pdfPath, true);

                var pdf = new FileSource(pdfPath);
                MuPdfWrapper.GeneratePagesAtHeight(pdf, thumbsFolder, 100);

                MuPdfWrapper.GeneratePagesAtScale(pdf, pagesFolder, 1.0f);
                var pageCount = MuPdfWrapper.CountPages(pdf);

                Data.Instance.Lesson = new Lesson(lessonName, pageCount);

                await Application.Current.Dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            if (!controller.IsCanceled)
                            {
                                Catalog.Instance.ReInit("MainPage");
                                Catalog.Instance.ReInit("FreeNotesPage");
                                Catalog.Instance.ReInit("QuestionsPage");
                                Catalog.Instance.NavigateTo("MainPage");
                            }
                        }
                        ),
                    DispatcherPriority.Normal
                    );

                await controller.CloseAsync();
            });

            if (controller.IsCanceled)
            {
                try
                {
                    var directory = Data.GetLessonFolder(lessonName);
                    Directory.Delete(directory, true);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                // Save lesson just after its creation
                (Application.Current.MainWindow.DataContext as NineManagement).SaveFile.Execute(null);
            }

            CloseFlyout();
        }

        internal async Task ShowProgressDialogAddLessonShare(string title, string message, string lessonName,
            byte[] bytePDF, bool justSave)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);

            var controller = await mainWindow.ShowProgressAsync(title, message);
            controller.SetCancelable(true);

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            await Task.Run(async () => //Task.Run automatically unwraps nested Task types!
            {
                // Control the name
                var baseName = lessonName;
                var i = 1;
                while (Directory.Exists(Data.GetLessonFolder(lessonName)))
                    lessonName = baseName + " " + i++;

                // Initial model data
                var lessonFolder = Data.GetLessonFolder(lessonName);
                var thumbsFolder = Data.GetThumbsFolder(lessonName);
                var pagesFolder = Data.GetPagesFolder(lessonName);
                var pdfPath = Data.GetPdfFile(lessonName);

                Directory.CreateDirectory(lessonFolder);
                Directory.CreateDirectory(thumbsFolder);
                Directory.CreateDirectory(pagesFolder);
                File.WriteAllBytes(pdfPath, bytePDF);

                var pdf = new FileSource(pdfPath);
                MuPdfWrapper.GeneratePagesAtHeight(pdf, thumbsFolder, 100);

                MuPdfWrapper.GeneratePagesAtScale(pdf, pagesFolder, 1.0f);
                var pageCount = MuPdfWrapper.CountPages(pdf);

                if (!justSave)
                {
                    Data.Instance.Lesson = new Lesson(lessonName, pageCount);
                    await Application.Current.Dispatcher.BeginInvoke(
                        new Action(
                            () =>
                            {
                                if (!controller.IsCanceled)
                                    Catalog.Instance.ReplaceAndGoTo("MainPage");
                            }
                            ),
                        DispatcherPriority.Normal
                        );
                }
                else
                {
                    await Application.Current.Dispatcher.BeginInvoke(
                        new Action(
                            () =>
                            {
                                Data.Instance.Lesson = new Lesson(lessonName, pageCount);
                                Save();
                                if (!controller.IsCanceled && Catalog.Instance.CurrentPageName != "MainPage")
                                    Catalog.Instance.ReplaceAndGoTo("HomePage");
                            }
                            ),
                        DispatcherPriority.Normal
                        );
                }

                await controller.CloseAsync();
            });

            if (controller.IsCanceled)
            {
                try
                {
                    var directory = Data.GetLessonFolder(lessonName);
                    Directory.Delete(directory, true);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                // Save lesson just after its creation
                (Application.Current.MainWindow.DataContext as NineManagement).SaveFile.Execute(null);
            }

            CloseFlyout();
        }

        internal async void ShowConfirmDialogReceiveLesson(string title, string message, string lessonName,
            byte[] lessonPDF)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);

            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Oui",
                NegativeButtonText = "Non"
            };

            var result = await mainWindow.ShowMessageAsync(title, message,
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                Save();
                Close();

                await ShowProgressDialogAddLessonShare(
                    "Veuillez patienter...",
                    "   Votre leçon est en cours de création !",
                    lessonName,
                    lessonPDF,
                    false
                    );
            }
            else
            {
                var result2 =
                    await
                        mainWindow.ShowMessageAsync("Notification de réception d'une leçon",
                            "   Voulez-vous la sauvegarder ?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result2 == MessageDialogResult.Affirmative)
                {
                    await ShowProgressDialogAddLessonShare(
                        "Veuillez patienter...",
                        "   Votre leçon est en cours de création !",
                        lessonName,
                        lessonPDF,
                        true
                        );
                }
            }
        }

        private ICommand _search;

        public ICommand SearchTag
        {
            get
            {
                if (_search == null)
                    _search = new RelayCommand(() => Search());
                return _search;
            }
        }

        public void Search()
        {
            CloseFlyout();
        }

        private ICommand _openFile;

        public ICommand OpenFile
        {
            get
            {
                if (_openFile == null)
                    _openFile = new RelayCommand(() => Open());
                return _openFile;
            }
        }

        private void Open()
        {
            var mainWindow = (Application.Current.MainWindow as MetroWindow);
            (mainWindow.Flyouts.Items[0] as Flyout).IsOpen = false;

            CloseAllPage();
            Catalog.Instance.NavigateTo("HomePage");
        }

        private void CloseAllPage()
        {
            Catalog.Instance.RemovePage("MainPage");
            Catalog.Instance.RemovePage("FreeNotesPage");
            Catalog.Instance.RemovePage("QuestionsPage");
        }

        private ICommand _aboutClick;

        public ICommand AboutClick
        {
            get
            {
                if (_aboutClick == null)
                    _aboutClick = new RelayCommand(() => About());
                return _aboutClick;
            }
        }

        private async void About()
        {
            var mainWindow = (Application.Current.MainWindow as MetroWindow);
            await
                mainWindow.ShowMessageAsync("À propos",
                    "Nine est développé par une équipe d'étudiants du département informatique de l'INSA Rennes : \n\t Gautier COLAJANNI \n\t Julien MARCOU \n\t Pierre POILANE \n\t Paul RIVIÈRE \n"
                    + "Groupe encadré par Éric ANQUETIL (IRISA/INSA) et conseillé par la société Excense\n"
                    + "Spécifications rédigées avec l'aide de Frank CHASSING, Amandine FOUILLET et Thomas GIRAUDEAU.");
        }

        private ICommand _mainWindowClose;

        public ICommand MainWindowClose
        {
            get
            {
                if (_mainWindowClose == null)
                    _mainWindowClose = new RelayCommand(() => MainClose());
                return _mainWindowClose;
            }
        }

#if DEBUG
        public void MainClose()
        {
            Application.Current.Shutdown();
        }
#else
        internal async void MainClose()
        {
            CloseFlyout();

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Oui",
                NegativeButtonText = "Non",
                FirstAuxiliaryButtonText = "Annuler fermeture",
                AnimateShow = true,
                AnimateHide = false
            };

            var mainWindow = (App.Current.MainWindow as MetroWindow);
            var result = await mainWindow.ShowMessageAsync("Fermeture de NINE",
                "Voulez-vous sauvegarder votre leçon ?",
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            if (result != MessageDialogResult.FirstAuxiliary)
            {
                bool saveLesson = result == MessageDialogResult.Affirmative;

                if (saveLesson)
                    Save();

                App.Current.Shutdown();
            }
        }
#endif

#if DEBUG
        public void MainClose(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
#else
        public async void MainClose(object sender, CancelEventArgs e)
        {
            CloseFlyout();

            e.Cancel = true;

            if (Catalog.Instance.CurrentPageName == "MainPage" || Catalog.Instance.CurrentPageName == "FreeNotesPage" || Catalog.Instance.CurrentPageName == "QuestionsPage")
            {
                CloseFlyout();

                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    FirstAuxiliaryButtonText = "Annuler fermeture",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var mainWindow = (App.Current.MainWindow as MetroWindow);
                var result = await mainWindow.ShowMessageAsync("Fermeture de NINE",
                    "   Voulez-vous sauvegarder vos documents ?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                if (result != MessageDialogResult.FirstAuxiliary)
                {
                    bool saveLesson = result == MessageDialogResult.Affirmative;

                    if (saveLesson)
                        Save();
                    App.Current.Shutdown();
                }
            }
            else
            {
                App.Current.Shutdown();
            }
        }
#endif

        private ICommand _closeFile;

        public ICommand CloseFile
        {
            get
            {
                if (_closeFile == null)
                    _closeFile = new RelayCommand(() => Close());
                return _closeFile;
            }
        }


#if DEBUG
        internal void Close()
        {
            CloseFlyout();
            Data.Instance.Lesson = null;
            Catalog.Instance.NavigateTo("HomePage");
            CloseAllPage();
        }
#else
        internal async void Close()
        {
            if (Catalog.Instance.CurrentPageName == "MainPage" || Catalog.Instance.CurrentPageName == "FreeNotesPage" || Catalog.Instance.CurrentPageName == "QuestionsPage" )
            {
                CloseFlyout();

                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    FirstAuxiliaryButtonText = "Annuler fermeture",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var mainWindow = (App.Current.MainWindow as MetroWindow);
                var result = await mainWindow.ShowMessageAsync("Fermeture de la leçon",
                    "Voulez-vous sauvegarder votre leçon ?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                if (result != MessageDialogResult.FirstAuxiliary)
                {
                    bool saveLesson = result == MessageDialogResult.Affirmative;

                    if (saveLesson)
                        Save();

                    Data.Instance.Lesson = null;
                    Catalog.Instance.NavigateTo("HomePage");
                    CloseAllPage();
                }
            }
        }
#endif

        #endregion

        #region LessonAppBar

        private ICommand _renameLesson;

        public ICommand RenameLesson
        {
            get
            {
                if (_renameLesson == null)
                    _renameLesson = new RelayCommand(Rename());
                return _renameLesson;
            }
            private set
            {
                _renameLesson = value;
                RaisePropertyChanged();
            }
        }

        private Action Rename()
        {
            return () =>
            {
                CloseFlyout();

                var lessonStack = Catalog.Instance.GetPage("HomePage").DataContext as LessonStack;
                lessonStack.RenameLessonTrue();
            };
        }

        private ICommand _deleteLesson;

        public ICommand DeleteLesson
        {
            get
            {
                if (_deleteLesson == null)
                    _deleteLesson = new RelayCommand(RemoveLesson());
                return _deleteLesson;
            }
            private set
            {
                _deleteLesson = value;
                RaisePropertyChanged();
            }
        }

        private Action RemoveLesson()
        {
            return () =>
            {
                CloseFlyout();

                var lessonStack = Catalog.Instance.GetPage("HomePage").DataContext as LessonStack;
                lessonStack.DeleteLesson();
            };
        }

        private ICommand _shareLesson;

        public ICommand ShareLesson
        {
            get
            {
                if (_shareLesson == null)
                    _shareLesson = new RelayCommand(() => Share());
                return _shareLesson;
            }
            private set
            {
                _shareLesson = value;
                RaisePropertyChanged();
            }
        }

        private void Share()
        {
            CloseFlyout();

            var mainWindow = (Application.Current.MainWindow as MetroWindow);
            var flyout = mainWindow.Flyouts.Items[2] as Flyout;
            flyout.IsOpen = true;

            var shareLesson = flyout.DataContext as ShareContent;
            shareLesson.GetMembersConnected();
            shareLesson.IsLesson = true;
        }

        #endregion
    }
}