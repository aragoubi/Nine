using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.Lessons.Contents.Processing;
using Nine.Roles;
using Nine.Sharing;
using Nine.ViewModels.Controls;
using Nine.ViewModels.Windows;
using Nine.Views.Pages;
using LinkIOcsharp;
using LinkIOcsharp.model;
using System.Data.Linq;

namespace Nine.Tools
{
    /// <summary>
    ///     One-instance class who handle ALL the communications received by the YAPI with dynamic context and protective
    ///     reception
    /// </summary>
    public class YEvents
    {
        private static readonly int nbInstances = 0;
        private static YEvents _instance;

        public YEvents(NineManagement nm)
        {
            if (nbInstances > 0)
                throw new Exception("YEvents can be instantiated more than once");

            _instance = this;

            LIO = LinkIOImp.Instance;
            Data = Data.Instance;
            Catalog = Catalog.Instance;
            NM = nm;
        }

        public Catalog Catalog { get; set; }
        public Data Data { get; set; }
        public LinkIOcsharp.LinkIO LIO { get; set; }
        public NineManagement NM { get; set; }

        public bool Connected
        {
            get { return !Data.IsNullUser; }
        }

        public bool IsStudent
        {
            get { return Connected && Data.Role == Role.STUDENT; }
        }

        public bool IsTeacher
        {
            get { return Connected && Data.Role == Role.TEACHER; }
        }

        public bool LessonExists
        {
            get
            {
                return Connected && !Data.IsNullLesson && Catalog.Exists("MainPage") && Catalog.Exists("FreeNotesPage") &&
                       Catalog.Exists("QuestionsPage");
            }
        }

        public MainPage MainPage
        {
            get
            {
                if (!LessonExists)
                    throw new Exception("Not authorized access to this page.");
                return (Catalog.GetPage("MainPage") as MainPage);
            }
        }

        public FreeNotesPage FreeNotesPage
        {
            get
            {
                if (!LessonExists)
                    throw new Exception("Not authorized access to this page.");
                return (Catalog.GetPage("FreeNotesPage") as FreeNotesPage);
            }
        }

        public QuestionsPage QuestionsPage
        {
            get
            {
                if (!LessonExists)
                    throw new Exception("Not authorized access to this page.");
                return (Catalog.GetPage("QuestionsPage") as QuestionsPage);
            }
        }

        public void Register()
        {
            // When a lesson is shared by someone
            LIO.on("lesson", (e) => { OnLessonReceived(e as Event); });

            LIO.on("slide-layer", (e) => { OnSlideLayerReceived(e as Event); });
            LIO.on("freenotes-layer", (e) => { OnFreeNotesLayerReceived(e as Event); });
            LIO.on("question-layer", (e) => { OnQuestionLayerReceived(e as Event); });

            RegisterTeacherEvents();
            RegisterStudentEvents();
        }

        private void RegisterTeacherEvents()
        {
            LIO.on("slide-index", (e) => { OnAskingSlideIndex(e as Event); });

            LIO.on("graphical-answer", (e) => { OnGraphicalAnswer(e as Event); });
            LIO.on("quiz-answer", (e) => { OnQuizAnswer(e as Event); });
        }

        private void RegisterStudentEvents()
        {
            LIO.on("new-page-index", (e) => { OnPageIndexReceived(e as Event); });

            LIO.on("slide-pointer", (e) => { OnSlidePointerStrokeReceived(e as Event); });
            LIO.on("question-pointer", (e) => { OnQuestionPointerStrokeReceived(e as Event); });

            // Only students can receive pointer (fix double pointer-writing when the teacher draw and receive the same stroke)
            LIO.on("quiz-ask", (e) => { OnQuizReceived(e as Event); });
            LIO.on("graphical-ask", (e) => { OnGraphicalReceived(e as Event); });
        }

        private bool OnQuestionPointerStrokeReceived(Event ev)
        {
            if (!(IsStudent && LessonExists))
                return true;

            var sharedPointer = ev.get<SharedPointer>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                // If we're not sync or on Question mode, we don't care
                if (Catalog.CurrentPageName != "QuestionsPage")
                    return;

                int position;
                try
                {
                    position = Data.Lesson.Exercises.GetPositionOfUID(sharedPointer.Offset);
                    // We select the good slide
                    (QuestionsPage.LessonPage as Exercises).GoingToPage(position);
                    // We tell the PointerManager that we want to show a new stroke
                    PointerManager.Instance.DrawStroke(StrokeConverter.ToWindowsStroke(sharedPointer.Stroke));
                }
                catch (Exception)
                {
                    Console.WriteLine("UID (Exercise Pointer) not found.");
                }
            });

            return true;
        }

        private bool OnLessonReceived(Event ev)
        {
            var lessonReceived = ev.get<SharedLesson>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        NM.ShowConfirmDialogReceiveLesson("Réception d'une leçon",
                            "   " + lessonReceived.OwnerName + " vous a envoyé une leçon, voulez-vous l'ouvrir ?",
                            lessonReceived.Name,
                            lessonReceived.PDF
                            );
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnPageIndexReceived(Event ev)
        {
            if (!(IsStudent && LessonExists))
                return true;

            Lesson lesson = null;
            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        lesson = (MainPage.LessonPage.DataContext as Lesson);
                        lesson.TeacherPageIndex = (int)ev.get<int>();

                        // If the receiver is ok to switch slide, we update his current page
                        if (lesson.IsSync.HasValue && lesson.IsSync.Value && Catalog.CurrentPageName != "QuestionsPage")
                        {
                            lesson.ChangeCurrentPage(lesson.TeacherPageIndex);
                        }
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnSlideLayerReceived(Event ev)
        {
            if (!LessonExists)
                return true;

            var layerReceived = ev.get<SharedParallelLayer>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        if (Catalog.CurrentPageName == "FreeNotesPage")
                            Catalog.NavigateTo("MainPage");

                        try
                        {
                            layerReceived.AddThem(Data.Instance.Lesson.Slides);
                            (MainPage.LessonPage.DataContext as Lesson).LayerStack.SetLayers();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnSlidePointerStrokeReceived(Event ev)
        {
            if (!(IsStudent && LessonExists))
                return true;

            var sharedPointer = ev.get<SharedPointer>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        var lesson = (MainPage.LessonPage.DataContext as Lesson);

                        // If we're not sync or on Question mode, we don't care
                        if (!lesson.IsSync.Value || Catalog.CurrentPageName != "MainPage")
                            return;

                        // We redirect to the correct view (beacause, if we are on FreeNotesPage in Sync mode, we want to follow
                        Catalog.NavigateTo("MainPage");

                        // We select the good slide
                        lesson.ChangeCurrentPage(sharedPointer.Offset);

                        // We tell the PointerManager that we want to show a new stroke
                        PointerManager.Instance.DrawStroke(StrokeConverter.ToWindowsStroke(sharedPointer.Stroke));
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnFreeNotesLayerReceived(Event ev)
        {
            var layerReceived = ev.get<SharedParallelLayer>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        layerReceived.AddThem(Data.Instance.Lesson.GlobalNotes);
                        FreeNotesPage.LayerStackDC.SetLayers();
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnAskingSlideIndex(Event ev)
        {
            if (!(IsTeacher && LessonExists))
                return true;

            var asking = ev.get<AskingSlide>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        var list = new List<User>();
                        list.Add(User.getTestUsers().Find(u => u.ID == asking.User + ""));
                        LinkIOImp.Instance.send(
                            "new-page-index",
                            (MainPage.LessonPage.DataContext as Lesson).CurrentPageIndex
                            , list);
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnQuestionLayerReceived(Event ev)
        {
            if (!LessonExists)
                return true;

            var layerReceived = ev.get<SharedExerciseLayer>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        layerReceived.AddThem(Data.Instance.Lesson.Exercises);
                        QuestionsPage.LayerStackDC.SetLayers();
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        /// <summary>
        ///     Called when quiz answer answer is received
        /// </summary>
        /// <param name="ev">The ev.</param>
        /// <returns></returns>
        private bool OnQuizAnswer(Event ev)
        {
            if (!(IsTeacher && LessonExists))
                return true;

            var answer = ev.get<AnswerLayer<QuizLayer>>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        try
                        {
                            var exercise = Data.Instance.Lesson.Exercises.GetContentByUID(answer.UID) as QuizContent;
                            if (exercise.Accept)
                            {
                                (exercise.Processing as QuizProcessing).CollectLayer(answer.Layer);
                                (exercise.AnswerLayer as QuizAnswerLayer).NbParticipants++;
                            }
                        }
                        catch (Exception e)
                        {
                            // UID not found
                            Console.WriteLine(e.Message);
                        }
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        /// <summary>
        ///     Called when graphical answer answer is received
        /// </summary>
        /// <param name="ev">The ev.</param>
        /// <returns></returns>
        private bool OnGraphicalAnswer(Event ev)
        {
            if (!(IsTeacher && LessonExists))
                return true;

            var answer = ev.get<AnswerLayer<BasicLayer>>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        try
                        {
                            var exercise = Data.Instance.Lesson.Exercises.GetContentByUID(answer.UID);
                            if (exercise.Accept)
                            {
                                (exercise.Processing as GraphicalProcessing).CollectLayer(answer.Layer);
                                (exercise.AnswerLayer as GraphicalAnswerLayer).NbParticipants++;
                            }
                        }
                        catch (Exception e)
                        {
                            // UID not found
                            Console.WriteLine(e.Message);
                        }
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        /// <summary>
        ///     Called when quiz exercise is received
        /// </summary>
        /// <param name="ev">The ev.</param>
        /// <returns></returns>
        private bool OnQuizReceived(Event ev)
        {
            if (!(IsStudent && LessonExists))
                return true;

            var exercise = ev.get<SharedQuizExercise>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        try
                        {
                            // If exercise already exists we authorize to be answered again
                            Data.Lesson.Exercises.GetContentByUID(exercise.UID).Answered = false;
                            return;
                        }
                        catch
                        {
                        }

                        exercise.AddIt(Data.Instance.Lesson.Exercises);


                        (QuestionsPage.LessonPage as Exercises).Questions.Add(
                            new KeyValuePair<int, DescEx>(
                                (QuestionsPage.LessonPage as Exercises).Questions.Count,
                                new DescEx(
                                    exercise.ExerciseName,
                                    (Data.Lesson.Exercises.Contents[Data.Lesson.Exercises.Contents.Count - 1] as
                                        ExerciseContent).Kind)));

                        QuestionsPage.LessonPage.ChangeCurrentPage(Data.Lesson.Exercises.Contents.Count - 1);
                        QuestionsPage.LayerStackDC.SetLayers();
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        /// <summary>
        ///     Called when graphical exercise is received on the student
        /// </summary>
        /// <param name="ev">The ev.</param>
        /// <returns></returns>
        private bool OnGraphicalReceived(Event ev)
        {
            if (!(IsStudent && LessonExists))
                return true;

            var exercise = ev.get<SharedGraphicExercise>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        try
                        {
                            // If exercise already exists we authorize to be answered again
                            Data.Lesson.Exercises.GetContentByUID(exercise.UID).Answered = false;
                            return;
                        }
                        catch
                        {
                        }

                        exercise.AddIt(Data.Lesson.Exercises);
                        // Last layer of last exercise
                        var exHolder = Data.Lesson.Exercises;
                        var lastContent = exHolder.Contents.Count - 1;

                        (QuestionsPage.LessonPage as Exercises).Questions.Add(
                            new KeyValuePair<int, DescEx>(
                                (QuestionsPage.LessonPage as Exercises).Questions.Count,
                                new DescEx(
                                    exercise.ExerciseName,
                                    (Data.Instance.Lesson.Exercises.Contents[
                                        Data.Instance.Lesson.Exercises.Contents.Count - 1] as ExerciseContent).Kind)));

                        QuestionsPage.LessonPage.ChangeCurrentPage(lastContent);
                        QuestionsPage.LayerStackDC.SetLayers();
                        QuestionsPage.LayerStackDC.CurrentLayerIndex = exHolder.Contents[lastContent].Layers.Count - 1;
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }

        private bool OnPointerStrokeReceived(Event ev)
        {
            if (!(IsStudent && LessonExists && Catalog.CurrentPageName == "QuestionsPage"))
                return true;

            var sharedPointer = ev.get<SharedPointer>();

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        int position;

                        try
                        {
                            position = Data.Instance.Lesson.Exercises.GetPositionOfUID(sharedPointer.Offset);

                            // We select the good slide
                            (QuestionsPage.LessonPage as Exercises).GoingToPage(position);

                            // We tell the PointerManager that we want to show a new stroke
                            PointerManager.Instance.DrawStroke(StrokeConverter.ToWindowsStroke(sharedPointer.Stroke));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("UID (Exercise Pointer) not found.");
                        }
                    }
                    ),
                DispatcherPriority.Normal
                );

            return true;
        }
    }
}