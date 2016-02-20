using System;
using System.IO;
using Nine.Lessons;
using Nine.MVVM;
using Nine.Roles;
using LinkIOcsharp.model;

namespace Nine
{
    /// <summary>
    ///     Data is a Singleton class who store the current lesson and the current user
    /// </summary>
    public class Data : BaseViewModel, IDisposable
    {
        private static Data _instance;
        private static readonly string nineFolderName = "Nine";
        private static readonly string nineThumbsFolderName = "thumbs";
        private static readonly string ninePagesFolderName = "pages";
        private static readonly string ninePdfFileName = "lesson.pdf";
        private static readonly string nineDataFileName = "data.nine";

        private static readonly string _nineFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nineFolderName);

        /// <summary>
        ///     Gets or sets the lesson.
        /// </summary>
        /// <value>
        ///     The lesson.
        /// </value>
        private Lesson _lesson;

        private User _user;

        private Data()
        {
        }

        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Data();
                    Instance.Role = Role.STUDENT;
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        public static string NineFolder
        {
            get { return _nineFolder; }
        }

        public bool IsNullUser
        {
            get { return _user == null; }
        }

        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged();
            }
        }

        public Role Role { set; get; }

        public bool IsNullLesson
        {
            get { return _lesson == null; }
        }

        public Lesson Lesson
        {
            get { return _lesson; }
            set
            {
                _lesson = value;
                RaisePropertyChanged();
            }
        }

        public void Dispose()
        {
            Instance = null;
        }

        public static string GetLessonFolder(string lessonName)
        {
            return Path.Combine(NineFolder, lessonName);
        }

        public static string GetThumbsFolder(string lessonName)
        {
            return Path.Combine(NineFolder, lessonName, nineThumbsFolderName);
        }

        public static string GetPagesFolder(string lessonName)
        {
            return Path.Combine(NineFolder, lessonName, ninePagesFolderName);
        }

        public static string GetThumbFile(string lessonName, int pageNumber)
        {
            return Path.Combine(NineFolder, lessonName, nineThumbsFolderName, pageNumber + ".png");
        }

        public static string GetPageFile(string lessonName, int pageNumber)
        {
            return Path.Combine(NineFolder, lessonName, ninePagesFolderName, pageNumber + ".png");
        }

        public static string GetPdfFile(string lessonName)
        {
            return Path.Combine(NineFolder, lessonName, ninePdfFileName);
        }

        public static string GetDataFile(string lessonName)
        {
            return Path.Combine(NineFolder, lessonName, nineDataFileName);
        }
    }
}