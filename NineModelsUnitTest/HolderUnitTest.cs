using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nine.Lessons;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;

namespace Nine.Tests
{
    [TestClass]
    public class HolderUnitTest
    {
        [TestMethod]
        public void TestUidConcordance()
        {
            const int nbPages = 2;
            var lesson = new Lesson("", nbPages);

            const string layerName = "Calque de test";
            var uid = (lesson.Slides.Contents[0] as ParallelContent).AddLayer(layerName);

            Assert.AreEqual(uid, lesson.Slides.GetUid(layerName));
        }

        /// <summary>
        /// Tests the layers coherence for name
        /// </summary>
        [TestMethod]
        public void TestLayersNameCoherence()
        {
            const int nbPages = 2;
            var lesson = new Lesson("", nbPages);

            // We want to retrieve the default UID layer
            Assert.AreEqual(1, lesson.Slides.LayerNames.Count);
            var uid = lesson.Slides.LayerNames.Keys.First();

            const string newName = "New layer name";

            // We only have one layer (we've tested it)
            // We test if that first layer has the new name
            foreach (var content in lesson.Slides.Contents) // We have 2 pages
                Assert.AreNotEqual(newName, content.Layers[0].Name);

            // A layer is renamed by the Holder !
            (lesson.Slides as ParallelHolder).LayerNames[uid] = newName;

            // We only have one layer (we've tested it)
            // We test if that first layer has the new name
            foreach (var content in lesson.Slides.Contents) // We have 2 pages
                Assert.AreEqual(newName, content.Layers[0].Name);
        }

        /// <summary>
        /// Checks the possibility of renaming a layer by the same name
        /// </summary>
        [TestMethod]
        public void TestRenamingLayerWithSameName()
        {
            var lesson = new Lesson("", 1);

            // We want to retrieve the default UID layer
            Assert.AreEqual(1, lesson.Slides.LayerNames.Count);
            var uid = lesson.Slides.LayerNames.Keys.First();

            string currentName = lesson.Slides.Contents[0].Layers[0].Name;

            // We try to rename with the same (no exception is attented)
            lesson.Slides.Contents[0].RenameLayer(uid, currentName);

            // We check if the first layer of the first slide has is Name to currentName
            Assert.AreEqual(currentName, lesson.Slides.Contents[0].Layers[0].Name);
        }

        /// <summary>
        /// Tests the layers reaction when we try to attribute a name already attributed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Slide name already referenced.")]
        public void TestNameAlreadyReferenced()
        {
            var lesson = new Lesson("", 1);
            const string name = "My new layer";

            // We want to retrieve the default UID layer
            Assert.AreEqual(1, lesson.Slides.LayerNames.Count);
            var firstLayerUid = lesson.Slides.LayerNames.Keys.First();
            var scondLayerUid = lesson.Slides.Contents[0].AddLayer(name);

            // We try to rename the 1st layer by the name of the 2nd (exception expected)
            lesson.Slides.Contents[0].RenameLayer(firstLayerUid, name);
            Assert.Fail();
        }

        /// <summary>
        /// Tests the layers coherence for visibility
        /// </summary>
        [TestMethod]
        public void TestLayersVisibilityCoherence()
        {
            const int nbPages = 2;
            var lesson = new Lesson("", nbPages);

            // We want to retrieve the default UID layer
            Assert.AreEqual(1, lesson.Slides.LayerNames.Count);
            var uid = lesson.Slides.LayerVisibility.Keys.First();

            // Currently, the first (& only) slide has this visibility
            var isVisible = lesson.Slides.LayerVisibility[uid];

            // We check if every layer has the good visibility (first coherence)
            foreach (var content in lesson.Slides.Contents) // We have 2 pages
                Assert.AreNotEqual(!isVisible, content.Layers[0].IsDisplayed);

            // A Layer has is Visibility changed if one of his siblings change
            lesson.Slides.Contents[0].Layers[0].IsDisplayed = !isVisible;

            // We test if that layer (on every slide) has the toggled visibility (2nd coherence)
            foreach (var content in lesson.Slides.Contents) // We have 2 pages
                Assert.AreEqual(!isVisible, content.Layers[0].IsDisplayed);
        }
    }
}
