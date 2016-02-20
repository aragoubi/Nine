using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nine.Lessons;

namespace Nine.Tests
{
    [TestClass]
    public class LessonUnitTest
    {
        [TestMethod]
        public void TestCreateLesson()
        {
            const string name = "Leçon de test";
            var lesson = new Lesson(name, 5);

            Assert.AreEqual(name, lesson.Name);
        }

        [TestMethod]
        public void TestReachSlides()
        {
            const int nbPages = 5;
            var lesson = new Lesson("", nbPages);

            for (int i = 0; i < nbPages; i++)
                Assert.IsNotNull(lesson.Slides.Contents[i]);
        }
    }
}
