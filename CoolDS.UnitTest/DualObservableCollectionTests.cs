using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoolDS.UnitTest
{
    [TestFixture]
    public class DualObservableCollectionTests
    {
        [Test]
        public void DefaultConstructorShouldSetVisualSizeTo0()
        {
            var collection = new DualObserableCollection<int>();
            Assert.AreEqual(collection.VisualSize, 0);
        }

        [Test]
        public void SetVisualSizeInConstructor()
        {
            var collection = new DualObserableCollection<int>(5);
            Assert.AreEqual(collection.VisualSize, 5);
        }

        [TestCase(1, Description = "VisualSize less than collection size")]
        [TestCase(5, Description = "VisualSize greater than collection size")]
        public void ConstructorWithFullParameters(int visualSize)
        {
            var source = new List<int>(){1, 2, 3};
            var collection = new DualObserableCollection<int>(source, visualSize);
            Assert.AreEqual(collection.VisualSize, visualSize);
        }

        [Test]
        public void CanNotMove1StepToLeftIfCollectionEmpty()
        {
            var collection = new DualObserableCollection<int>();
            Assert.IsFalse(collection.CanMove1StepToLeft);
        }

        [Test]
        public void CanMove1StepToLeftByConstructorWithCollectionAndSmallerVisualSize()
        {
            var collection = new DualObserableCollection<int>(new List<int>{1, 2, 3, 4, 5}, 2);
            Assert.IsTrue(collection.CanMove1StepToLeft);
        }

        [TestCase(3, Description = "Steps less equal to maximum steps to Left")]
        [TestCase(5, Description = "More than maximum steps to Left")]
        public void MoveStepsToLeft(int steps)
        {
            var collection = new DualObserableCollection<int>(new List<int> { 1, 2, 3, 4, 5 }, 2);
            collection.MoveStepsToLeft(steps);
            var firstVisible = collection.VisualElements.First();
            var lastVisibile = collection.VisualElements.Last();
            Assert.AreEqual(4, firstVisible, "First visual element should be 4");
            Assert.AreEqual(5, lastVisibile, "Last visual element should be 5");
        }

        [TestCase(null, 0, Description = "Empty collection")]
        [TestCase(new int[] { 1, 2, 3 }, 2, Description = "VisualSize is less than count")]
        [TestCase(new int[] { 1, 2, 3 }, 5, Description = "VisualSize is greater than count")]
        public void CanNotMove1StepToRightJustAfterConstruction(ICollection<int> source, int visualSize)
        {
            var collection = new DualObserableCollection<int>(source, visualSize);
            Assert.IsFalse(collection.CanMove1StepToRight);
        }

        [TestCase(3, Description = "Steps less equal to maximum steps to right")]
        [TestCase(5, Description = "More than maximum steps to right")]
        public void MoveStepsToRight(int steps)
        {
            var collection = new DualObserableCollection<int>(new List<int> { 1, 2, 3, 4, 5 }, 2);
            collection.MoveStepsToLeft(5);

            collection.MoveStepsToRight(steps);

            var firstVisible = collection.VisualElements.First();
            var lastVisibile = collection.VisualElements.Last();
            Assert.AreEqual(1, firstVisible, "First visual element should be 1");
            Assert.AreEqual(2, lastVisibile, "Last visual element should be 2");
        }
    }
}