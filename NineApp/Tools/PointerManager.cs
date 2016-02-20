using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Threading;
using Nine.Roles;
using Nine.Sharing;
using Nine.ViewModels.Controls;
using Nine.Views.Pages;
using LinkIOcsharp;

namespace Nine.Tools
{
    /// <summary>
    /// The PointerManager encapsulate the needed methods to handle the pointer on each role
    /// </summary>
    public class PointerManager
    {
        /// <summary>
        ///     PointerManager is a Singleton class
        /// </summary>
        private static PointerManager _instance;

        /// <summary>
        ///     The total duration of the draw OR the deletion
        /// </summary>
        private readonly int _duration;

        /// <summary>
        ///     The quantum : how many ms we wait before launch a new "tick"
        /// </summary>
        private readonly int _quantum;

        public Dictionary<int, List<StylusPoint>> toBeDrawed;

        private PointerManager()
        {
            toBeDrawed = new Dictionary<int, List<StylusPoint>>();

            _quantum = 10;
            _duration = 800;
        }

        public static PointerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Instance = new PointerManager();
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        /// <summary>
        ///     The LayerStack reference
        /// </summary>
        private LayerStack LS
        {
            get
            {
                var CurrentPage = Catalog.Instance.CurrentPageName;
                var Page = Catalog.Instance.GetPage(CurrentPage);
                LayerStack layerStack = null;
                if (CurrentPage == "MainPage")
                {
                    Application.Current.Dispatcher.Invoke(
                        () => { layerStack = (Page as MainPage).LayerStack.DataContext as LayerStack; });
                }
                else if (CurrentPage == "FreeNotesPage")
                {
                    Application.Current.Dispatcher.Invoke(
                        () => { layerStack = (Page as FreeNotesPage).LayerStack.DataContext as LayerStack; });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(
                        () => { layerStack = (Page as QuestionsPage).LayerStack.DataContext as LayerStack; });
                }

                return layerStack;
            }
        }

        /// <summary>
        ///     We just received (outsite event) and we want to draw and then delete the stroke.
        /// </summary>
        /// <param name="fullStroke">The full stroke.</param>
        public void DrawStroke(Stroke fullStroke)
        {
            var nbPts = HowManyPointPerTick(fullStroke.StylusPoints.Count);

            // Preparation of the pointer stroke
            var points = new StylusPointCollection();
            points.Add(fullStroke.StylusPoints[0]);
            fullStroke.StylusPoints.RemoveAt(0);
            var stroke = new Stroke(points);

            var uid = Guid.NewGuid().GetHashCode();
            toBeDrawed.Add(uid, new List<StylusPoint>());

            foreach (var pt in fullStroke.StylusPoints)
                toBeDrawed[uid].Add(pt);

            // Replicates the drawing attributes of that stroke
            stroke.DrawingAttributes = fullStroke.DrawingAttributes;

            // Adding our new Stroke to the ViewModel
            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () => { LS.PointerStrokes.Add(stroke); }
                    ),
                DispatcherPriority.Render
                );

            // Creation of the Timer who will add the points
            var insertionTimer = new Timer(_quantum);
            var deletionTimer = new Timer(_quantum);


            // Preparation of the deletion timer, InsertionTimer will start it at the end
            deletionTimer.Elapsed += (src, args) => TimedDeletion(
                src,
                new Tuple<StrokeCollection, Stroke, int>(
                    LS.PointerStrokes,
                    stroke,
                    nbPts));

            insertionTimer.Elapsed += (src, args) => TimedInsertion(
                src, new Tuple<Timer, int, StylusPointCollection, int>(
                    deletionTimer,
                    uid,
                    points,
                    nbPts));
            insertionTimer.Start();
        }

        /// <summary>
        ///     A Stroke has been received (inner event) and we trigger the Deletion event.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        public void StrokeAdded(Stroke stroke)
        {
            var currentPageName = Catalog.Instance.CurrentPageName;
            if (currentPageName == "MainPage" || currentPageName == "QuestionsPage")
            {
                var currentLesson = currentPageName == "MainPage"
                    ? (Catalog.Instance.GetPage(currentPageName) as MainPage).LessonPage.DataContext as Lesson
                    : (Catalog.Instance.GetPage(currentPageName) as QuestionsPage).LayerStackDC.LessonViewModel;
                // We don't send very small strokes
                if (stroke.StylusPoints.Count > 2
                    && Data.Instance.Role == Role.TEACHER
                    && currentPageName != "FreeNotesPage"
                    && currentLesson.IsSync == true)
                {
                    // Packing and sending our stroke and context to share with students
                    LinkIOImp.Instance.send(
                        currentPageName == "MainPage" ? "slide-pointer" : "question-pointer",
                        new SharedPointer(
                            LS.LessonViewModel.CurrentPageIndex,
                            StrokeConverter.ToNineStroke(stroke)));
                }

                // Creation of a new timer, execution every quantum
                var timer = new Timer(_quantum);

                var nbPtsPerTick = HowManyPointPerTick(stroke.StylusPoints.Count);

                // Setup with the timer collection, StrokeCollection and the right Stroke
                timer.Elapsed += (src, args) => TimedDeletion(
                    src,
                    new Tuple<StrokeCollection, Stroke, int>(
                        LS.PointerStrokes,
                        stroke,
                        nbPtsPerTick));
                timer.Start();
            }
        }

        /// <summary>
        ///     What is done at each tick
        ///     We insert as many as required to build the received stroke
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="ctx">The CTX.</param>
        private static void TimedInsertion(object src, Tuple<Timer, int, StylusPointCollection, int> ctx)
        {
            var strokePoints = ctx.Item3;
            var howManyPointsPerTick = ctx.Item4;

            var remainingPoints = Instance.toBeDrawed[ctx.Item2].Count;
            var nbPts = howManyPointsPerTick;
            if (remainingPoints <= howManyPointsPerTick)
                nbPts = remainingPoints;

            Application.Current.Dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        var points = Instance.toBeDrawed;
                        for (var i = 0; i < nbPts; i++)
                        {
                            if (points.ContainsKey(ctx.Item2) && points[ctx.Item2].Count > 0)
                            {
                                strokePoints.Add(points[ctx.Item2][0]);
                                points[ctx.Item2].RemoveAt(0);
                            }
                            else break;
                        }
                    }
                    ),
                DispatcherPriority.Render
                );

            if (remainingPoints <= howManyPointsPerTick)
            {
                // Stopping the insertion timer
                (src as Timer).Stop();

                // Removing our reference
                Instance.toBeDrawed.Remove(ctx.Item2);

                // Starting the deletion timer
                ctx.Item1.Start();
            }
        }

        /// <summary>
        ///     At each tick, we remove the amount of point required
        ///     When the number of point is low, we delete the entire Stroke from the original StrokeCollection
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="ctx">The CTX.</param>
        private static void TimedDeletion(object src, Tuple<StrokeCollection, Stroke, int> ctx)
        {
            var strokes = ctx.Item1;
            var stroke = ctx.Item2;
            var howManyPointPerTick = ctx.Item3;

            // Last point, we halt our timer
            if (stroke.StylusPoints.Count <= howManyPointPerTick)
            {
                // Stopping the timer
                var timer = (Timer) src;
                timer.Stop();

                // Deleting the Stroke from the StrokeCollection
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(
                        () => { strokes.Remove(stroke); }
                        ),
                    DispatcherPriority.Render
                    );
            }
            else
            {
                // Deletion of the oldest point
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            try
                            {
                                for (var i = 0; i < howManyPointPerTick; i++)
                                    stroke.StylusPoints.RemoveAt(0);
                            }
                            catch (InvalidOperationException)
                            {
                            } // Next tick, we delete the stroke
                        }
                        ),
                    DispatcherPriority.Render
                    );
            }
        }

        /// <summary>
        ///     Returns the number of points to remove step by step to provide a fluid animation
        /// </summary>
        /// <param name="totalPoints">The total points.</param>
        /// <returns></returns>
        private int HowManyPointPerTick(int totalPoints)
        {
            var nb = totalPoints/(_duration/_quantum);
            return (nb < 1) ? 1 : nb;
        }
    }
}